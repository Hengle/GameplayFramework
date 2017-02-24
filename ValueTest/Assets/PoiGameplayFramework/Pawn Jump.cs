using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public partial class Pawn
    {
        protected bool QueryJump { get; set; }

        public void Jump()
        {
            QueryJump = true;
        }

        protected virtual void ApplyJump()
        {
            if (DataInfo.JumpCurrentStep < 0)
            {
                ///防止负值导致无限跳
                DataInfo.JumpCurrentStep = 0;
            }

            CheckGroundStatus();


            if (QueryJump && DataInfo.JumpCurrentStep < DataInfo.JumpMaxStep)
            {
                ///清除下落速度
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.velocity = (Vector3.up * DataInfo.JumpPower);
                DataInfo.JumpCurrentStep++;
            }
            
            QueryJump = false;
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

                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("CanStand"))
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
    }
}
