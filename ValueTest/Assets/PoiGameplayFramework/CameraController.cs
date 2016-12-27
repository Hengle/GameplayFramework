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


        public Transform EnemyTarget1;
        public Transform EnemyTarget2;

        public Vector3 CenterRectOffset = new Vector3(0,0,7.5f);

        // 仅在首次调用 Update 方法之前调用 Start
        private void Start()
        {

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
        }
    }
}