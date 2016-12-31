using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{

    /// <summary>
    /// 角色
    /// <para>Controller将每次移动命令存在Pawn中(根据Time.deltatime求出位移，在下次应用前叠加)，
    /// pawn每次FixedUpdate应用这些命令</para>
    /// </summary>
    public partial class Pawn:IMove
    {

        public MoveState CurrentState { get; set; }

        [SerializeField]
        float m_MovingTurnSpeed = 360;
        [SerializeField]
        float m_StationaryTurnSpeed = 180;
        
        [Range(1f, 4f)]
        [SerializeField]
        float m_GravityMultiplier = 2f;
        [SerializeField]
        float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField]
        float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        float m_GroundCheckDistance = 0.2f;


        bool m_IsGrounded;
        float m_OrigGroundCheckDistance = 0.5f;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        Vector3 CapsuleCenter => new Vector3(0,DataInfo.Height/2,0);
        CapsuleCollider m_Capsule;
        public bool IsCrouching { get; protected set; }

        /// <summary>
        /// 相机仰角
        /// </summary>
        public float CameraElevation { get; private set; } = -90f;
        /// <summary>
        /// 相机俯角
        /// </summary>
        public float CameraDipAngle { get; private set; } = 90f;

        /// <summary>
        /// 人物旋转角度
        /// </summary>
        public Quaternion m_CharacterTargetRot { get; private set; } = Quaternion.identity;
        /// <summary>
        /// 相机旋转角度
        /// </summary>
        public Quaternion m_FirstViewSlotTargetRot { get; private set; }
        public float CharatorAxisSmoothRatio { get; private set; } = 1;

        /// <summary>
        /// 下次移动距离
        /// </summary>
        public Stack<Vector3> NextMoveDistance { get; } = new Stack<Vector3>();
        public bool NextJump { get; protected set; }

        
        #region Move

        /// <summary>
        /// 在当前状态持续的时间
        /// </summary>
        public float DurationTimeInCurrentState { get;set; } = 0;

        private void RunStart()
        {
            State = PawnState.RunStart;

            float speed = DataInfo.Run.Current * DataInfo.Run.StartScale;
            Move(speed);
        }

        private void Move(float speed)
        {
            transform.Translate(0, 0, speed*Time.fixedDeltaTime, Space.Self);
        }

        /// <summary>
        /// move
        /// </summary>
        public virtual void Move()
        {
            PawnMoveFSM.OnUpdate(this,Time.deltaTime);
        }

        /// <summary>
        /// 开始移动
        /// </summary>
        public void MoveStart(float deltaTime)
        {
            float speed = DataInfo.Run.Current;
            Move(deltaTime, speed);
        }

        

        public void MoveStop(float deltaTime)
        {
            float speed = DataInfo.Run.Current;
            Move(deltaTime, speed);
        }

        public void Moving(float deltaTime)
        {
            float speed = DataInfo.Run.Current;
            Move(deltaTime, speed);
        }


        protected virtual void Move(float deltaTime, float speed)
        {
            

            ///读取下个目标点
            var next = NextMoveDistance.Peek();
            ///位移
            var tempdis = Vector3.MoveTowards(transform.position, next,
                                                speed * deltaTime);

            ///执行位移
            transform.Translate(tempdis, Space.Self);

            if (CheckArrive(next, 0.2f))
            {
                ///认为到达 移除坐标点
                NextMoveDistance.Pop();
            }
        }

        /// <summary>
        /// 检测是否到达目标点（移动中测试）
        /// </summary>
        /// <param name="next"></param>
        /// <param name="allowH">允许的高度差</param>
        /// <returns></returns>
        private bool CheckArrive(Vector3 next, float allowH)
        {
            ///检测平面位置
            var current = new Vector2(transform.position.x, transform.position.z);
            var target = new Vector2(next.x, next.z);

            if ((target - current).sqrMagnitude > 0.04)
            {
                return false;
            }

            ///检测高度
            float deltaH = transform.position.y - next.y;
            if (Math.Abs(deltaH) > allowH)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Axis

        public float NextTurnToAngle { get; set; }

        /// <summary>
        /// 自身旋转到目标角度
        /// </summary>
        public virtual void Turn()
        {
            //if (CurrentCmd)
            //{
            //    if (CurrentCmd.Angle != null)
            //    {
            //        NextTurnToAngle = (float)CurrentCmd.Angle;
            //    }
            //}

            var delta = NextTurnToAngle - transform.localEulerAngles.y;

            if (delta == 0) return;

            if (delta > 180) delta -= 360;
            if (delta < -180) delta += 360;

            var tempSpeed = DataInfo.TurnSpeed * Time.fixedDeltaTime;

            delta = delta.ClampIn(-tempSpeed, tempSpeed);

            transform.Rotate(0, delta, 0, Space.World);
        }

        #endregion

        #region Jump

        public void QueryJump()
        {
            NextJump = true;
        }

        protected virtual void ApplyJump()
        {
            
            
        }

        private void ResetJumpState()
        {
            DataInfo.JumpCurrentStep = 0;
        }

        private void TriggerEnter(Collider arg1, Collider arg2)
        {
            //if (arg1.tag == Tag.JumpReset)
            //{
            //    ResetJumpState();
            //}
        }

        /// <summary>
        /// 检测是否在地面
        /// </summary>
        protected void CheckGroundStatus()
        {
            RaycastHit hitInfo;

#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position , transform.position+ (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;

                var angle = Vector3.Angle(m_GroundNormal, Vector3.up);
                if (angle < 45)
                {
                    ResetJumpStep();
                    return;
                }

                if (hitInfo.collider.gameObject.layer == (int)Layer.Floor)
                {
                    ResetJumpStep();
                    return;
                }
            }
            else
            {
                //m_IsGrounded = false;
                //m_GroundNormal = Vector3.up;
                //m_Animator.applyRootMotion = false;
            }
        }

        /// <summary>
        /// 重置跳跃次数
        /// </summary>
        private void ResetJumpStep()
        {
            DataInfo.JumpCurrentStep = 0;
        }

        public void PlayAnim(MoveState state, MoveState lastState)
        {
            Debug.Log(state + "---" + lastState);
        }


        #endregion


    }
}

