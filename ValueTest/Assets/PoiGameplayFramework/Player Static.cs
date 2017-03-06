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
        public static Player Instance { get; set; }

        internal static void SetName(string name)
        {
            Instance.DataInfo.Name = name;
        }
    }
}
