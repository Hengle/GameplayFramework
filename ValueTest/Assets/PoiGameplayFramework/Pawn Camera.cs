using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色
    /// </summary>
    public partial class Pawn
    {
        #region CameraPositon

        /// <summary>
        /// 第一视角位置
        /// </summary>
        public Transform EyeCamaraPos;

        /// <summary>
        /// 第三视角位置
        /// </summary>
        public Transform ThirdCameraPos;

        /// <summary>
        /// 初始相机视角位置
        /// </summary>
        protected void InitEyeCameraPos()
        {
            EyeCamaraPos = transform.FindChild("EyeCamaraPos");
            ThirdCameraPos = transform.FindChild("ThirdCameraPos");
        }

        #endregion
    }
}
