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
        private static int tempinstanceID = 0;
        public static int InstanceID
        {
            get
            {
                if (DataInfo == null)
                {
                    return tempinstanceID;
                }
                else
                {
                    return DataInfo.ID;
                }
            }
            set
            {
                tempinstanceID = value;
                if (DataInfo != null)
                {
                    DataInfo.ID = value;
                }
            }
        }
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
