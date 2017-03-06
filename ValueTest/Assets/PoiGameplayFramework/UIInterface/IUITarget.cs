using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public interface IUITarget
    {
        /// <summary>
        /// 目标ID
        /// </summary>
        int ID { get; }
        /// <summary>
        /// 在屏幕中和鼠标的距离
        /// </summary>
        float magnitudeToMouse { get; }
        string Name { get; }
        Vector3 NamePosition { get; }

        /// <summary>
        /// 在屏幕中的坐标
        /// </summary>
        Vector3 ScreenPosition { get; }
        Transform transform { get; }
    }

}