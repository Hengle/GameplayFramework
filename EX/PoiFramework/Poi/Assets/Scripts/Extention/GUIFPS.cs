﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 原生GUI显示帧率
    /// </summary>
    public class GUIFPS:MonoBehaviour
    {
        // FPS
        private float oldTime;
        private int frame = 0;
        private static float frameRate = 0f;
        private const float INTERVAL = 0.5f;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isOn = true;

        void Start()
        {
            oldTime = Time.realtimeSinceStartup;
        }


        void Update()
        {
            frame++;
            float time = Time.realtimeSinceStartup - oldTime;
            if (time >= INTERVAL)
            {
                frameRate = frame / time;
                oldTime = Time.realtimeSinceStartup;
                frame = 0;
            }
        }

        /// <summary>
        /// 获取帧率
        /// </summary>
        /// <returns></returns>
        public static float GetFrameRate()
        {
            return frameRate;
        }


        void OnGUI()
        {
            if (isOn)
            {
                GUI.Label(new Rect(25, 25, 160, 20), "FPS : " + frameRate.ToString());
            }
        }
    }
}
