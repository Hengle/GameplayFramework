using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace Poi
{
    public partial class Character
    {
        protected override void ApplyTurn()
        {
            var delta = NextTurnToAngle - transform.localEulerAngles.y;

            if (delta == 0) return;

            if (delta > 180) delta -= 360;
            if (delta < -180) delta += 360;

            var tempSpeed = DataInfo.TurnSpeed * Time.fixedDeltaTime;

            delta = delta.ClampIn(-tempSpeed, tempSpeed);

            transform.Rotate(0, delta, 0, Space.World);
        }
    }
}
