using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Poi
{
    /// <summary>
    /// 玩家控制器
    /// <para>控制器思路：</para>
    /// <para>1,取得当前操作InputCommand，存入cmd 命令列表</para>
    /// <para>2,模拟延迟（单机为0），取得前N帧的操作 和当前时刻人物状态进行解析</para>
    /// <para>3,解析InputCommand。AI怪物使用行为树和AIPawn状态解析，联机角色服务器网间解析</para>
    /// <para>4,解析过得Command送入Pawn，类型为直接对Pawn的控制，例如转到某个角度，开始Run，结束Run，Run持续，
    /// 释放Skill等。
    /// </para>
    /// </summary>
    public class PlayerController:CharacterControllor
    {
        public enum TestCtrlType
        {
            A,B
        }
        public TestCtrlType CtrlType = TestCtrlType.B;

        public CameraController CamCtrl { get; protected set; }

        Stack<InputCommand> cmd = new Stack<InputCommand>();

        /// <summary>
        /// 延迟fixedtime数
        /// </summary>
        public int DelayFixedtime = 0;

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

        public bool LockCursor { get; private set; }

        private void Update()
        {
            
        }

        protected override void FixedUpdate()
        {
            //base.FixedUpdate();

            ///取得输入命令
            InputCommand next = new InputCommand();
            next.Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            next.Vertical = CrossPlatformInputManager.GetAxis("Vertical");

            next.Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            next.MouseX = CrossPlatformInputManager.GetAxis("Mouse X");
            next.MouseY = CrossPlatformInputManager.GetAxis("Mouse Y");

            InputCommand tempcmd = null;

            ///应用模拟延迟
            if (DelayFixedtime == 0)
            {
                tempcmd = next;
            }
            else
            {
                cmd.Push(next);
                if (cmd.Count > DelayFixedtime)
                {
                    tempcmd = cmd.Pop();
                }
            }

            if (tempcmd)
            {
                if (Pawn == null)
                {

                }
                else
                {
                    ///解析操作
                    ParseInputCommand(tempcmd);
                }     
            }
        }

        /// <summary>
        /// 将输入命令解析为对Pawn命令（AI中使用行为树或状态机解析）
        /// </summary>
        /// <param name="next">输入的命令</param>
        /// <returns></returns>
        private void ParseInputCommand(InputCommand next)
        { 
            ///解析所转向的角度
            Vector2 arrow = new Vector2(next.Horizontal, next.Vertical);
            if (arrow != Vector2.zero)
            {
                float angle = Vector2.Angle(Vector2.up, arrow);
                if (arrow.x < 0)
                {
                    angle = 360 - angle;
                }

                Pawn.NextTurnToAngle = angle;
            }

            ///计算移动
            if (arrow != Vector2.zero)
            {
                Pawn.Acceleration = arrow.magnitude;
            }
            else
            {
                Pawn.Acceleration = -0.1f;
            }
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
