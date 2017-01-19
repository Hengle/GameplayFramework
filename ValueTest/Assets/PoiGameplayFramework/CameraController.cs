using System;
using UnityEngine;

namespace Poi
{
    public class CameraController:MonoBehaviour
    {
        public Transform FollowTarget;

        /// <summary>
        /// 相机的父（自拍杆）
        /// </summary>
        public Transform Selfiestick;
        public Vector3 SelfiestickRotationOffset = new Vector3(20, 0, 0);

        public Transform EnemyTarget1;
        public Transform EnemyTarget2;

        public Vector3 speed = new Vector3(1, 2, 1);
        public Vector3 CenterRectOffset = new Vector3(0,0,-3.5f);


        public UpdateType UpdateType = UpdateType.LateUpdate;
        public LerpType LerpType = LerpType.Null;
        public float MaxSpeed = 15f;
        private float SmoothTime = 0.3f;

        private void Follow()
        {
            //Selfiestick.position = Vector3.SmoothDamp(Selfiestick.position, FollowTarget.position,
            //                        ref speed, SmoothTime, MaxSpeed);

            Selfiestick.position = Vector3.Lerp(Selfiestick.position, FollowTarget.position,Time.deltaTime);
        }

        // 加载脚本实例时调用 Awake
        private void Awake()
        {
            if (!transform.parent)
            {
                GameObject go = new GameObject(name + "-Selfiestick");
                transform.SetParent(go.transform);

                transform.localPosition = CenterRectOffset;
                transform.rotation = Quaternion.identity;
            }

            Selfiestick = transform.parent;

            Selfiestick.Rotate(SelfiestickRotationOffset.x, 0, 0);
            Selfiestick.Rotate(0, SelfiestickRotationOffset.y, 0, Space.World);
        }

        // 仅在首次调用 Update 方法之前调用 Start
        private void Start()
        {

        }

        /// <summary>
        /// 如果启用 Behaviour，则在每一帧都将调用 LateUpdate
        /// </summary>
        private void LateUpdate()
        {
            if (UpdateType == UpdateType.LateUpdate)
            {
                if (FollowTarget && Selfiestick)
                {
                    Follow();
                }
            }
        }

        private void Update()
        {
            if (UpdateType == UpdateType.Update)
            {
                if (FollowTarget && Selfiestick)
                {
                    Follow();
                }
            }
        }

        private void FixedUpdate()
        {
            if (UpdateType == UpdateType.FixedUpdate)
            {
                if (FollowTarget && Selfiestick)
                {
                    Follow();
                }
            }
        }

        public void Turn(Vector2 move)
        {
            Selfiestick.RotateAround(FollowTarget.position, Vector3.up, move.x);
            Selfiestick.RotateAround(FollowTarget.position, Vector3.right, -move.y);
        }
    }
}