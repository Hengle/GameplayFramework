using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 生命值
    /// </summary>
    public class HP : BaseProperty , IRestoreProperty
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





        #region 恢复

        /// <summary>
        /// 恢复时间间隔
        /// </summary>
        public float TimeInterval { get; set; }
        /// <summary>
        /// 每秒恢复
        /// </summary>
        public double RestorePerTime { get; set; }
        /// <summary>
        /// 距离下次恢复时间
        /// </summary>
        public float RestoreCooldownTime { get; set; }
        /// <summary>
        /// 禁用恢复时间
        /// </summary>
        public float RestoreDisableTime { get; set; }

        public void TickRestore(float time)
        {
            RestoreCooldownTime -= time;
            if (RestoreCooldownTime <= 0)
            {
                Restore();

                RestoreCooldownTime += TimeInterval;
            }
        }

        protected virtual void Restore()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public struct HPOnHitResult
    {
        public double current;
        public double overflowingDamage;
    }


}
