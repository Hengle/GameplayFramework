using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class Monster :Pawn
    {
        /// <summary>
        /// 怪物信息（数据模型）
        /// </summary>
        public new MonsterInfo DataInfo => dataInfo as MonsterInfo; 
    }
}
