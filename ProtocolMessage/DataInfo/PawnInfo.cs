using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{

    /// <summary>
    /// 角色信息
    /// </summary>
    public partial class PawnInfo:DataInfo
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public virtual PawnType PawnType => PawnType.Pawn;

        [ProtoMember(1)]
        public int ID { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        [ProtoMember(3)]
        public float Height { get; set; }
        [ProtoMember(4)]
        public float JumpPower { get; set; }
        /// <summary>
        /// 跳跃段数
        /// </summary>
        public int JumpCurrentStep { get; set; }
        /// <summary>
        /// 允许最大跳跃段数
        /// </summary>
        [ProtoMember(5)]
        public int JumpMaxStep { get; set; } = 1;
        [ProtoMember(6)]
        public string ModelName { get; set; }

        /// <summary>
        /// 当前人物是否在地面
        /// </summary>
        public bool IsGround => JumpCurrentStep == 0;

        /// <summary>
        /// 转动速度
        /// </summary>
        [ProtoMember(8)]
        public float TurnSpeed { get; set; } = 450;
        [ProtoMember(7)]
        public CoolDown ATKCD = new CoolDown(1);

        public HP HP { get; private set; } = new HP() { Max = 1,Current = 1};
        public MP MP { get; private set; } = new MP();
        public SpeedBase Walk { get; private set; } = new SpeedBase(PropertyType.WalkSpeed);
        [ProtoMember(9)]
        public SpeedBase Run { get; private set; } = new SpeedBase(PropertyType.RunSpeed);

        public bool IsDead => HP.Current <= 0;
        
        public IList<IRestoreProperty> RestoreProperties
        {
            get
            {
                return null;
            }
        }

        
    }
}
