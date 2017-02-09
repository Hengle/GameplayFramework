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
            var targetpos = transform.localToWorldMatrix.MultiplyPoint3x4(
                new Vector3(0, DataInfo.Height*2/3, 0.5f));

            GameObject proj = new GameObject(name + "-Projectile");
            proj.transform.position = targetpos;
            proj.transform.rotation = transform.rotation;


            var go = Resources.Load<GameObject>("Projectile/projectile");
            GameObject.Instantiate(go, proj.transform,false);

            var p =  proj.AddComponent<Projectile>();
            p.Target = new ArrowTarget();
            p.Speed = 1.0f;

            DataInfo.AttackCooldown.EnterCooling();
        }
    }
}
