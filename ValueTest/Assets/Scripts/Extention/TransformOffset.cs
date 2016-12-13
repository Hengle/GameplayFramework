using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 描述一个transform的位置和旋转偏移
    /// </summary>
    public struct TransformOffset
    {
        private Vector3 position;
        
        private Quaternion rotation;

        private Vector3 rotationV3;

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        /// <summary>
        /// 旋转的四元数形式
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        /// <summary>
        /// 旋转的欧拉角形式
        /// </summary>
        public Vector3 RotationV3
        {
            get
            {
                return rotationV3;
            }

            set
            {
                rotationV3 = value;
            }
        }
    }
}
