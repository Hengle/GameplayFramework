using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色移动
    /// </summary>
    public partial class Pawn
    {
        public float AttackCooldown => DataInfo.AttackCooldown.Current;

        internal void Attack()
        {
            var startpos = transform.localToWorldMatrix.MultiplyPoint3x4(
                new Vector3(0, DataInfo.Height*2/3, 0.5f));
            var targetpos = transform.localToWorldMatrix.MultiplyPoint3x4(
                new Vector3(0, DataInfo.Height * 2 / 3, 10f));

            GameObject proj = new GameObject(name + "-Projectile");
            proj.transform.position = startpos;
            proj.transform.rotation = transform.rotation;


            var go = Resources.Load<GameObject>("Projectile/projectile");
            var temp = GameObject.Instantiate(go, proj.transform,false);
            temp.name = "ProjectileModel";

            var p =  proj.AddComponent<Projectile>();
            p.Target = new PointTarget() { TargetWorldPosotion = targetpos};
            p.Speed = 1.0f;

            DataInfo.AttackCooldown.EnterCooling();
        }
    }
}
