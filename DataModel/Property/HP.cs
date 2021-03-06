﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 生命值
    /// </summary>
    public class HP : BaseProperty 
    {
        public override ValueChangedType ChangedType => ValueChangedType.TickChanged;


        public override PropertyType PropertyType => PropertyType.HP;

        public HPOnHitResult OnHit(double damage)
        {
            var temp = Current - damage;
            HPOnHitResult res = new HPOnHitResult();
            if (temp < 0)
            {
                res.overflowingDamage = -temp;
            }

            Current = temp < 0 ? 0 : temp;
            res.current = Current;
            return res;
        }

        public void AddHP(double addValue)
        {
            throw new NotImplementedException();
        }

    }

    public struct HPOnHitResult
    {
        public double current;
        public double overflowingDamage;
    }


}
