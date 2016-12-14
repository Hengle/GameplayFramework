using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public class PlayerController:CharacterControllor
    {


        Parameter para;
        /// <summary>
        /// 创建玩家角色
        /// </summary>
        public Pawn CreatePlayer()
        {
            var temp_pgo = GameObject.Find("para");
            para = temp_pgo?.GetComponent<Parameter>();

            GameObject go = GameObject.Instantiate(para.player);
            //go.AddComponent<DontDestroyOnLoad>();
            var controller = go.GetComponent<Animator>();
            controller.runtimeAnimatorController = para.controller;
            var p = go.AddComponent<Character>();

            //go.AddComponent<CharacterController>();

            Possess(p);

            return p;
        }

        private void Start()
        {
            string friendlyName = "PlayerController";
            gameObject.name = friendlyName;
            //gameObject.tag = friendlyName;
        }


    }
}
