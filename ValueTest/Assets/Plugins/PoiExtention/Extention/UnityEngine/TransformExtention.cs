using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class TransformExtention
    {
        /// <summary>
        /// 重置local Vector3.zero Quaternion.identity Vector3.one
        /// </summary>
        /// <param name="trans"></param>
        public static void ResetLocal(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        public static void ResetLocal(this Transform trans, Vector3 csale)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = csale;
        }

        /// <summary>
        /// 位置重合
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="tar"></param>
        public static void Apply(this Transform trans, Transform tar)
        {
            if (tar)
            {
                trans.position = tar.position;
                trans.eulerAngles = tar.eulerAngles;
                trans.localScale = tar.localScale;
            } 
        }

        public static void ApplyRotationY(this Transform trans, Transform tar)
        {
            if (tar)
            {
                trans.eulerAngles = new Vector3(trans.eulerAngles.x,
                    tar.eulerAngles.y,
                    trans.eulerAngles.z);
            }
        }
    }
}
