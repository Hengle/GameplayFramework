using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;

        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
