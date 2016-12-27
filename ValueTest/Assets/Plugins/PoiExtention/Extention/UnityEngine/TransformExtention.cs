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
    }
}
