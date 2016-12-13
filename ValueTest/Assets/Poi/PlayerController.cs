using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public class PlayerController:CharacterControllor
    {
        public static readonly PlayerController Instance = new PlayerController();

        /// <summary>
        /// 创建玩家角色
        /// </summary>
        public Pawn CreatePlayer()
        {
            GameObject go = new GameObject("Player");
            go.AddComponent<DontDestroyOnLoad>();
            var p = go.AddComponent<Character>();
            Possess(p);

            return p;
        }


    }
}
