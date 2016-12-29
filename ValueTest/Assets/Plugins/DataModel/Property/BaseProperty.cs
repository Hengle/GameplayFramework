using System;

namespace Poi
{
    public abstract class BaseProperty : DataProperty, IMaxLimit<int,float>,IRestoreProperty
    {
        /// <summary>
        /// 许可的最大值
        /// </summary>
        public int Max { get; set; }
        
        /// <summary>
        /// 当前值
        /// </summary>
        public float Current { get; set; }


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

    public abstract class BaseProperty2 : BaseProperty, IRangeProperty<int, float>
    {
        /// <summary>
        /// 许可的最小值
        /// </summary>
        public int Min { get; set; }
    }
}