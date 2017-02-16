using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class CameraExtention
    {
        /// <summary>
        /// 世界中一个点是否在相机内
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static bool IsAPointInACamera(this Camera cam, Vector3 worldPos)
        {
            Vector3 Viewpos;
            return cam.IsAPointInACamera(worldPos,out Viewpos);
        }

        /// <summary>
        /// 世界中一个点是否在相机内
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="worldPos"></param>
        /// <param name="Viewpos">相机空间坐标，XY标准化</param>
        /// <returns></returns>
        public static bool IsAPointInACamera(this Camera cam, Vector3 worldPos,
            out Vector3 Viewpos)
        {
            /// 是否在视野内
            bool result1 = false;
            Vector3 posViewport = cam.WorldToViewportPoint(worldPos);
            Viewpos = posViewport;
            //Debug.Log("posViewport:" + posViewport.ToString());

            Rect rect = new Rect(0, 0, 1, 1);
            result1 = rect.Contains(posViewport);

            //Debug.Log("result1:" + result1.ToString());
            // 是否在远近平面内

            bool result2 = false;
            if (posViewport.z >= cam.nearClipPlane && posViewport.z <= cam.farClipPlane)
            {
                result2 = true;
            }

            //Debug.Log("result2:" + result2.ToString());

            /// 综合判断
            bool result = result1 && result2;

            //Debug.Log("result:" + result.ToString());

            return result;
        }
    }
}
