using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    /// <summary>
    /// 冷却计时
    /// </summary>
    public partial struct CoolDown : IComparable<CoolDown>, IEquatable<Boolean>
    {
        /// <summary>
        /// 剩余冷却时间
        /// </summary>
        [ProtoMember(1)]
        public double CoolDownTime { get; private set; }
        /// <summary>
        /// 最大冷却时间
        /// </summary>
        [ProtoMember(2)]
        public double MaxCD { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxCD">最大冷却时间</param>
        /// <param name="CoolDownTime">剩余冷却时间</param>
        public CoolDown(double MaxCD,double CoolDownTime = 0)
        {
            this.MaxCD = MaxCD;
            this.CoolDownTime = CoolDownTime;
        }

        /// <summary>
        /// 检查是否冷却完成，如果冷却完成自动ReCD
        /// </summary>
        /// <param name="delta">本次时间间隔</param>
        /// <param name="autoReCD">是否自动重置CD</param>
        /// <returns></returns>
        public bool Check(double delta,bool autoReCD = false)
        {
            if (CoolDownTime > 0)
            {
                CoolDownTime -= delta;
            }

            if (CoolDownTime <= 0)
            {
                if (autoReCD)
                {
                    CoolDownTime = MaxCD;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 重置冷却时间
        /// </summary>
        /// <param name="cooldownTime"></param>
        public void ReCD(double? cooldownTime = null)
        {
            CoolDownTime = cooldownTime??MaxCD;
        }

        /// <summary>
        /// 比较剩余冷却时间大小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CoolDown other)
        {
            if (CoolDownTime > other.CoolDownTime)
            {
                return 1;
            }
            else if (CoolDownTime == other.CoolDownTime)
            {
                return 0;
            }
            return -1;
        }

        public bool Equals(bool other)
        {
            var res = (bool)this;
            return res == other;
        }

        public void Update(double delta)
        {
            if (CoolDownTime > 0)
            {
                CoolDownTime -= delta;
            }
        }

        /// <summary>
        /// 增加剩余冷却时间
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static CoolDown operator +(CoolDown cd,double delta)
        {
            cd.CoolDownTime += delta;
            return cd;
        }

        /// <summary>
        /// 减少剩余冷却时间
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static CoolDown operator -(CoolDown cd, double delta)
        {
            cd.CoolDownTime -= delta;
            return cd;
        }

        /// <summary>
        /// 剩余时间小于等于0，返回true
        /// </summary>
        /// <param name="cd"></param>
        public static implicit operator bool(CoolDown cd)
        {
            if (cd.CoolDownTime > 0)
            {
                return false;
            }
            return true;
        }
    }
}

