using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
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

        /// <summary>
        /// 模型中心
        /// </summary>
        public Transform Chest { get; protected set; }

        bool m_IsGrounded;
        float m_OrigGroundCheckDistance = 0.5f;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        Vector3 CapsuleCenter => new Vector3(0,DataInfo.Height/2,0);
        CapsuleCollider m_Capsule;
        public bool IsCrouching { get; protected set; }
        public Transform LocalCenter { get; protected set; }
        public void InitTransform()
        {
            EyeCamaraPos = transform.FindChild("EyeCamaraPos");
            ThirdCameraPos = transform.FindChild("ThirdCameraPos");
            //LocalCenter = transform.FindChild("LocalCenter");
            //if (!LocalCenter)
            //{
            //    GameObject go = new GameObject("LocalCenter");
            //    go.transform.SetParent(transform);
            //    go.transform.ResetLocal();
            //    go.transform.localPosition = new Vector3(0, DataInfo.Height * 2 / 3, 0);
            //}

            ///初始模型中心点
            Chest = Animator.GetBoneTransform(HumanBodyBones.Chest);
            if (!Chest)
            {
                GameObject go = new GameObject("Chest");
                go.transform.SetParent(transform);
                go.transform.ResetLocal();

                if (DataInfo)
                {
                    go.transform.localPosition = new Vector3(0, DataInfo.Height / 2, 0);
                }
                Chest = go.transform;
            }

            Head = Animator.GetBoneTransform(HumanBodyBones.Head);
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

        /// <summary>
        /// 更新移动
        /// <para>Translate移动一定要放在FixedUpdate中，有效防止物理碰撞时抖动</para>
        /// </summary>
        private void FixedUpdateTransform()
        {
            ApplyJump();

            ApplyTurn();

            ApplyMove();
        }

        /// <summary>
        /// 在当前状态持续的时间
        /// </summary>
        public float DurationTimeInCurrentState { get; protected set; } = 0;
        public Transform Head { get; protected set; }

        public virtual void Move(Trans trans)
        {
            transform.position = new Vector3(trans.x, transform.position.y, trans.z);
            transform.rotation = new Quaternion(trans.qx, trans.qy, trans.qz, trans.qw);
        }

        public virtual void Move(double serverTime, Trans trans)
        {
            Move(trans);
        }
    }
}
