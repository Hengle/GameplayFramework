using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public partial class Character
    {
        protected override void ApplyMove()
        {
            base.ApplyMove();

            if (Animator)
            {
                Animator.SetFloat("Acceleration", Acceleration);
                Animator.SetFloat("Speed", DataInfo.Run.Current);
            }
        }
    }
}
