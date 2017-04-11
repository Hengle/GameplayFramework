using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
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
    /// <para>4,解析过得Command直接操作Pawn，类型为直接对Pawn的控制，例如转到某个角度，开始Run，结束Run，Run持续，
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

        Stack<InputCMD> CMD = new Stack<InputCMD>();

        /// <summary>
        /// 延迟fixedtime数
        /// </summary>
        public int DelayFixedtime = 5;

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
        public Vector2 lastMousePosition { get; private set; }

        /// <summary>
        /// 按帧取得外设输入，因为输入不能受TimeScale影响
        /// <para>某些输入的操作在Pawn中按FixedUpdate执行</para>
        /// <para>相机的操作按LateUpdate执行</para>
        /// </summary>
        protected override void Update()
        {
            if (CommandTool.IsShow)
            {
                return;
            }
            UpdateTarget(Time.deltaTime);

            #region 相机控制是即时的

            var mousePos = new Vector2(CrossPlatformInputManager.GetAxis("Mouse X"),
                CrossPlatformInputManager.GetAxis("Mouse Y"));

            if (Input.GetMouseButton(1))
            {
                CamCtrl?.Turn(mousePos);
            }

            CamCtrl?.ScaleDistance(Input.mouseScrollDelta);

            #endregion

            bool needSendCMD = false;
            InputCMD cmd0 = new InputCMD();
            cmd0.Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            needSendCMD |= cmd0.Jump;

            cmd0.IsAttact = Input.GetMouseButton(0);
            if (Pawn?.DataInfo.ATKCD.Check(Time.deltaTime)??false)
            {
                ///攻击冷却完毕，如果攻击则重置冷却时间
                if (cmd0.IsAttact)
                {
                    Pawn.DataInfo.ATKCD.ReCD();
                }
            }
            else
            {
                ///攻击冷却中，过滤掉工具操作
                cmd0.IsAttact = false;
            }
            needSendCMD |= cmd0.IsAttact;

            #region 解析所转向的角度和加速度

            ///解析所转向的角度
            Vector2 arrow = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
                CrossPlatformInputManager.GetAxis("Vertical"));
            if (arrow != Vector2.zero)
            {
                float angle = Vector2.Angle(Vector2.up, arrow);

                ///angle只有正直
                if (arrow.x < 0)
                {
                    ///当逆时针旋转时，将角度转换为顺时针
                    angle = 360 - angle;
                }

                float cur = CamCtrl.GetCurrentForward();

                angle += cur;

                cmd0.NextAngle = angle;

                needSendCMD = true;
            }

            ///计算加速度
            if (arrow != Vector2.zero)
            {
                cmd0.Acceleration = arrow.magnitude;

                needSendCMD = true;
            }
            else
            {
                cmd0.Acceleration = -1f;
            }

            #endregion

            if (needSendCMD)
            {
                GM.WriteToServer(cmd0);
            }

            InputCMD tempcmd = null;

            ///应用模拟延迟
            if (DelayFixedtime == 0)
            {
                tempcmd = cmd0;
            }
            else
            {
                CMD.Push(cmd0);
                if (CMD.Count > DelayFixedtime)
                {
                    tempcmd = CMD.Pop();
                }
            }

            if (tempcmd != null)
            {
                ///解析操作
                ParseInputCommand(tempcmd);      
            }
        }

        /// <summary>
        /// 更新目标
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void UpdateTarget(float deltaTime)
        {
            var tempTarget = new Target();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.GetPoint(1000), Color.red, Time.deltaTime);

            RaycastHit res;
            if (Physics.Raycast(ray, out res, 1000, LayerMask.GetMask("CanStand")))
            {
                tempTarget.Point = res.point;
            }
            else
            {
                tempTarget.Point = ray.GetPoint(1000);
            }

            tempTarget.LockedTargets = UI.GetLockedTargets();

            Target = tempTarget;
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
