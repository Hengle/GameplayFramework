using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
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


            if (QueryJump && DataInfo.JumpCurrentStep < DataInfo.JumpMaxStep)
            {
                ///清除下落速度
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.velocity = (Vector3.up * DataInfo.JumpPower);
                DataInfo.JumpCurrentStep++;
            }

            Animator?.SetFloat("SpeedY", Rigidbody.velocity.y);

            QueryJump = false;
        }
    }
}
