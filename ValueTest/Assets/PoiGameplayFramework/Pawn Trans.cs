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
    public partial class Pawn
    {
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

        public float CurrentSpeed
        {
            get
            {
                return DataInfo.Run.Speed;
            }
        }

        

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
        public Vector3 NextMoveDistance { get; protected set; }
        public bool NextJump { get; protected set; }




        /// <summary>
        /// 更新移动
        /// <para>Translate移动一定要放在FixedUpdate中，有效防止物理碰撞时抖动</para>
        /// </summary>
        private void FixedUpdateTransform()
        {
            if (PlayerController?.CtrlType == PlayerController.TestCtrlType.A)
            {
                ApplyMove();

                ApplyJump();
            }
            else
            {
                ApplyTurn();
            }
        }

        

        #region Move

        /// <summary>
        /// 下次移动
        /// </summary>
        public void NextMove(Vector3 moveDir)
        {
            ///去掉Y值
            moveDir = moveDir.ZeroY();

            AnimUpdateMove(moveDir);

            var currentSpeed = CurrentSpeed;
            NextMoveDistance += moveDir.normalized * currentSpeed * Time.deltaTime;

        }

        protected virtual void AnimUpdateMove(Vector3 moveDir)
        {
            if (Animator)
            {
                Animator.SetFloat("Vertical", moveDir.z);
                Animator.SetFloat("Horizontal", moveDir.x);
            }
        }

        /// <summary>
        /// 应用移动
        /// </summary>
        protected virtual void ApplyMove()
        {
            
        }

        #endregion

        #region Axis


        /// <summary>
        /// 超界检测
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, CameraElevation, CameraDipAngle);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        public void UpdateAxis(float xRot, float yRot)
        {
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_FirstViewSlotTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            m_FirstViewSlotTargetRot = ClampRotationAroundXAxis(m_FirstViewSlotTargetRot);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CharacterTargetRot,
                CharatorAxisSmoothRatio);
            //FirstViewSlot.localRotation = Quaternion.Slerp(FirstViewSlot.localRotation, m_FirstViewSlotTargetRot,
            //    charatorManager.Host.Config.CharatorAxisSmoothRatio);
        }

        

        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters
            Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            Animator.SetBool("Crouch", IsCrouching);
            Animator.SetBool("OnGround", m_IsGrounded);
            if (!m_IsGrounded)
            {
                Animator.SetFloat("Jump", Rigidbody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                Animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (m_IsGrounded && move.magnitude > 0)
            {
                Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                Animator.speed = 1;
            }
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

        #endregion

        public float NextTurnToAngle { get; protected set; }

        /// <summary>
        /// 自身渲染到目标角度
        /// </summary>
        protected virtual void ApplyTurn()
        {
            if (CurrentCmd)
            {
                if (CurrentCmd.Angle != null)
                {
                    NextTurnToAngle = (float)CurrentCmd.Angle;
                }
            }

            var delta = NextTurnToAngle - transform.localEulerAngles.y;

            if (delta == 0) return;

            if (delta > 180) delta -= 360;
            if (delta < -180) delta += 360;

            var tempSpeed = DataInfo.TurnSpeed * Time.fixedDeltaTime;

            delta = delta.ClampIn(-tempSpeed, tempSpeed);
            
            transform.Rotate(0, delta, 0,Space.World);
        }
    }
}
