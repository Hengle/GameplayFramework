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
        public Vector3 ViewPos { get; private set; }
        public bool IsInCamera { get; private set; } = false;

        /// <summary>
        /// 判断是否在相机内
        /// </summary>
        public void UpdateInCamera()
        {
            Vector3 viewPos;
            bool res = Camera.main.IsAPointInACamera(transform.position, out viewPos);
            ViewPos = viewPos;
            if (res != IsInCamera)
            {
                IsInCamera = !IsInCamera;
                if (IsInCamera)
                {
                    UI.AddPawn(this);
                }
                else
                {
                    UI.RemovePawn(this);
                }
            }
        }
    }
}
