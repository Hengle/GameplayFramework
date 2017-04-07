using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ProtoBuf;
using System.Collections;

namespace Poi
{
    /// <summary>
    /// 人形角色
    /// </summary>
    public partial class Player : Character
    {
        private static int tempinstanceID = 0;
        public static int InstanceID
        {
            get
            {
                if (DataInfo == null)
                {
                    return tempinstanceID;
                }
                else
                {
                    return DataInfo.ID;
                }
            }
            set
            {
                tempinstanceID = value;
                if (DataInfo != null)
                {
                    DataInfo.ID = value;
                }
            }
        }

        public static void ChangeModel(string name)
        {
            DataInfo.ModelName = name;

            Vector3 pos = Instance?.transform.position??Vector3.zero;
            Quaternion rotation = Instance?.transform.rotation ?? Quaternion.identity;

            Destroy(Instance?.gameObject);
            var p = CreatePlayer(DataInfo);

            p.transform.position = pos;
            p.transform.rotation = rotation;

            ///朝向初始化
            p.NextTurnToAngle = p.transform.eulerAngles.y;

            Instance = p;

            GM.PlayerController.Possess(p);

            var msg = new ModelChange()
            {
                instanceID = InstanceID,
                ModelName = DataInfo.ModelName,
            };

            GM.WriteToServer(msg);
        }

        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public static new PlayerInfo DataInfo => Instance?.dataInfo as PlayerInfo;

        public static Player Instance { get; set; }

        internal static void SetName(string name)
        {
            DataInfo.Name = name;
            if (GM.LineMode == LineMode.Online)
            {
                var msg = new NameChange()
                {
                    instanceID = InstanceID,
                    Name = name,
                };
                GM.WriteToServer(msg);
            }
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(SyncTrans());
        }

        private IEnumerator SyncTrans()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (GM.Instance.Server != null && GM.Instance.Server.IsConnected)
                {
                    var msg = new TransSync()
                    {
                        instanceID = DataInfo.ID,
                        trans = transform.ToTrans()
                    };
                    GM.Instance.Server.Write(msg);
                }
            }
        }

        public static Player CreatePlayer(PlayerInfo info)
        {
            GameObject go = GM.CreatePawnGameObject(info.ModelName, GameObject.FindWithTag(nameof(PoiTag.PlayerStart))?.transform);

            var p = go.AddComponent<Player>();

            p.Init(info);

            ///朝向初始化
            p.NextTurnToAngle = p.transform.eulerAngles.y;
            return p;
        }
    }

    public static class TransEX
    {
        public static Trans ToTrans(this Transform trans)
        {
            return new Trans()
            {
                x = trans.position.x,
                y = trans.position.y,
                z = trans.position.z,
                qx = trans.rotation.x,
                qy = trans.rotation.y,
                qz = trans.rotation.z,
                qw = trans.rotation.w,
            };
        }
    }
}
