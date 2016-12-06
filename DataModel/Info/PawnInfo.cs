using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class PawnInfo:DataInfo
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public virtual PawnType PawnType => PawnType.Pawn;

        public HP HP { get; private set; } = new HP() { Max = 1,Current = 1};
        public MP MP { get; private set; } = new MP();
        public Speed WalkSpeed { get; private set; } = new Speed(PropertyType.WalkSpeed);
        public Speed RunSpeed { get; private set; } = new Speed(PropertyType.RunSpeed);

        public bool IsDead => HP.Current <= 0;
        
    }
}
