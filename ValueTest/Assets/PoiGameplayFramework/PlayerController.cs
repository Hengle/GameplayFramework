﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Poi
{
    public class PlayerController:CharacterControllor
    {
        public CameraController CamCtrl { get; protected set; }

        protected override void Start()
        {
            string friendlyName = "PlayerController";
            gameObject.name = friendlyName;
            //gameObject.tag = friendlyName;

            var cam = Camera.main;
            if (!cam)
            {
                cam = new Camera();
                cam.name = "Main Camera";
                cam.tag = "Main Camera";
            }
            CamCtrl = cam.GetComponentIfNullAdd<CameraController>();

            CamCtrl.FollowTarget = transform;
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
                Pawn?.QueryJump();
            }

            bool crouch = Input.GetKey(KeyCode.C);

            GetMoveDirection();

            GetAxis();
        }

        public override void Init()
        {
            
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