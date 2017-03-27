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
        public static int InstanceID { get; internal set; }
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public static new PlayerInfo DataInfo => Instance.dataInfo as PlayerInfo;

        public static Player Instance { get; set; }

        internal static void SetName(string name)
        {
            DataInfo.Name = name;
        }
    }
}
