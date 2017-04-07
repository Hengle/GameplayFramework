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
    public partial class Pawn:IUITarget
    {
        /// <summary>
        /// 相机空间坐标
        /// </summary>
        public Vector3 ViewPos { get; private set; }

        public bool IsInCamera { get; private set; } = false;
        /// <summary>
        /// 屏幕位置
        /// </summary>
        public Vector3 ScreenPosition { get; protected set; }
        /// <summary>
        /// 在屏幕中距离鼠标的距离
        /// </summary>
        public float magnitudeToMouse { get; protected set; }

        public int ID => DataInfo.ID;

        public string Name => DataInfo.Name;

        public Vector3 NamePosition { get; protected set; }

        /// <summary>
        /// 判断是否在相机内
        /// </summary>
        public void UpdateInCamera()
        {
            Vector3 viewPos;
            bool res = Camera.main.IsAPointInACamera(Chest.position, out viewPos);
            IsInCamera = res;
            ViewPos = viewPos;
            ScreenPosition = Camera.main.ViewportToScreenPoint(viewPos);

            var vector3 = new Vector3(0, DataInfo.Height + 0.1f, 0);
            NamePosition = Camera.main.WorldToScreenPoint(transform.position + vector3);

            ///求在屏幕住和游标的距离
            magnitudeToMouse = ((Vector2)ScreenPosition - (Vector2)Input.mousePosition).magnitude;

            if (UI.PawnDic.ContainsKey(ID) != IsInCamera)
            {
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
