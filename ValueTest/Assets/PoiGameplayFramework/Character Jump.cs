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


            Animator?.SetFloat("SpeedY", Rigidbody.velocity.y);
            Animator?.SetBool("IsGround", DataInfo.IsGround);
        }
    }
}
