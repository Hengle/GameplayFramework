using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 人形角色
    /// </summary>
    public partial class Player : Character
    {
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public new PlayerInfo DataInfo => dataInfo as PlayerInfo;

        
    }
}
