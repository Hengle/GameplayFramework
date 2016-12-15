using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// 物体跟随
    /// </summary>
    public class FollowTarget:MonoBehaviour
    {
        [SerializeField]
        private Transform tar;
        /// <summary>
        /// 跟随的目标
        /// </summary>
        public Transform Tar
        {
            get
            {
                return tar;
            }

            set
            {
                tar = value;
            }
        }
        
        /// <summary>
        /// 插值幅度
        /// </summary>
        public int LerpScale
        {
            get
            {
                return lerpScale;
            }

            set
            {
                lerpScale = value;
            }
        }

        /// <summary>
        /// 刷新方法
        /// </summary>
        public UpdateType UpdateType = UpdateType.FixedUpdate;
        /// <summary>
        /// 插值方式
        /// </summary>
        public LerpType LerpType = LerpType.Lerp;
        [SerializeField]
        [Range(1, 30)]
        private int lerpScale = 10;

        private float lerp => lerpScale*Time.deltaTime;

        public void Update()
        {
            if (Check(UpdateType.Update))
            {
                Follow();
            }
        }

        private void Follow()
        {
            if (tar)
            {
                switch (LerpType)
                {
                    case LerpType.Lerp:
                        transform.position = Vector3.Lerp(transform.position, tar.position, lerp);
                        transform.rotation = Quaternion.Lerp(transform.rotation, tar.rotation, lerp);
                        transform.localScale = Vector3.Lerp(transform.localScale, tar.localScale, lerp);
                        break;
                    case LerpType.LerpUnclamped:

                        transform.position = Vector3.LerpUnclamped(transform.position, tar.position, lerp);
                        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, tar.rotation, lerp);
                        transform.localScale = Vector3.LerpUnclamped(transform.localScale, tar.localScale, lerp);

                        break;
                    default:
                        break;
                }
            }
        }

        public void FixedUpdate()
        {
            if (Check(UpdateType.FixedUpdate))
            {
                Follow();
            }
        }

        public void LateUpdate()
        {
            if (Check(UpdateType.LateUpdate))
            {
                Follow();
            }
        }

        bool Check(UpdateType t)
        {
            return (UpdateType & t) == t;
        }
    }

    public enum LerpType
    {
        Lerp,
        LerpUnclamped,
    }
}

/// <summary>
/// 刷新类型
/// </summary>
[Flags]
public enum UpdateType
{
    Update = 0x1,
    LateUpdate = 0x2,
    FixedUpdate = 0x4,
}