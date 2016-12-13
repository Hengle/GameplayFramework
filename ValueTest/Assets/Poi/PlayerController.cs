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

        Parameter para;
        /// <summary>
        /// 创建玩家角色
        /// </summary>
        public Pawn CreatePlayer()
        {
            var temp_pgo = GameObject.Find("para");
            para = temp_pgo?.GetComponent<Parameter>();

            GameObject go = new GameObject("Player");
            go.AddComponent<DontDestroyOnLoad>();
            var p = go.AddComponent<Character>();
            Possess(p);

            return p;
        }


    }
}
