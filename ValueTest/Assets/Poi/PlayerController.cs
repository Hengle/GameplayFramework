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

            PawnInfo info = new PawnInfo()
            {
                Height = 1.6f,
                JumpPower = 12f,
            };

            info.Run.Speed = 3f;

            p.Init(info);
            //go.AddComponent<CharacterController>();

            Possess(p);

            UpdateCamera();

            return p;
        }

        void UpdateCamera()
        {
            var cam = Camera.main;
            if (!cam)
            {
                cam = new Camera();
                cam.name = "Main Camera";
                cam.tag = "Main Camera";
            }


            FollowTarget f = cam.GetComponentIfNullAdd<FollowTarget>();

            f.Tar = Pawn.ThirdCameraPos;
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

        public bool LockCursor { get; private set; }

        private void Update()
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Pawn?.Jump();
            }

            bool crouch = Input.GetKey(KeyCode.C);

            GetMoveDirection();

            GetAxis();
        }

        private void GetAxis()
        {
            if (Input.GetMouseButton(1))
            {
                float yRot = CrossPlatformInputManager.GetAxis("Mouse X");
                float xRot = CrossPlatformInputManager.GetAxis("Mouse Y");

                Pawn?.UpdateAxis(xRot, yRot);
            }    
        }

        /// <summary>
        /// 获取玩家移动方向
        /// </summary>
        private void GetMoveDirection()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");


            var moveDir = new Vector3(h, 0, v);
            Pawn?.NextMove(moveDir);
        }

        void SetCursorLock(bool value)
        {
            LockCursor = value;
            if (!LockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
