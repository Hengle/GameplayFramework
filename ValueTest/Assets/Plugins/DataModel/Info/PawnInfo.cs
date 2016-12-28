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
        public SpeedBase Walk { get; private set; } = new SpeedBase(PropertyType.WalkSpeed);
        public SpeedBase Run { get; private set; } = new SpeedBase(PropertyType.RunSpeed);

        public bool IsDead => HP.Current <= 0;
        
        public IList<IRestoreProperty> RestoreProperties
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 身高
        /// </summary>
        public float Height { get; set; }
        public float JumpPower { get; set; }
        /// <summary>
        /// 跳跃段数
        /// </summary>
        public int JumpCurrentStep { get; set; }
        /// <summary>
        /// 允许最大跳跃段数
        /// </summary>
        public int JumpMaxStep { get; set; } = 1;

        /// <summary>
        /// 当前人物是否在地面
        /// </summary>
        public bool IsGround => JumpCurrentStep == 0;

        public float TurnSpeed { get; set; } = 1;
    }
}
