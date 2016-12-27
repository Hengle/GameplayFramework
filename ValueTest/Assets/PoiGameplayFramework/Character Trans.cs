using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 人形角色
    /// <para>你可以任意的继承扩展，但是不建议单独为Player继承一个类，Player应该和其他网络玩家类型保持一致，
    /// 用PlayerController区别功能。</para>
    /// </summary>
    public partial class Character
    {
        protected override void ApplyJump()
        {
            base.ApplyJump();

            if (DataInfo.JumpCurrentStep < 0)
            {
                ///防止负值导致无限跳
                DataInfo.JumpCurrentStep = 0;
            }

            CheckGroundStatus();


            if (NextJump && DataInfo.JumpCurrentStep < DataInfo.JumpMaxStep)
            {
                ///清除下落速度
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.velocity = (Vector3.up * DataInfo.JumpPower);
                DataInfo.JumpCurrentStep++;
            }

            if (Animator)
            {
                Animator.SetBool("Crouch", IsCrouching);
                Animator.SetBool("OnGround", DataInfo.IsGround);
                if (!DataInfo.IsGround)
                {
                    Animator.SetFloat("Jump", Rigidbody.velocity.y);
                }
            }

            NextJump = false;
        }


        protected override void ApplyMove()
        {
            transform.Translate(NextMoveDistance);

            NextMoveDistance = Vector3.zero;
        }
    }
}
