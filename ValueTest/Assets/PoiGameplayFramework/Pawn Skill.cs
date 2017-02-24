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
                new Vector3(0, DataInfo.Height * 2 / 3, 0.5f));

            CustomAttactProj proj = CreateProjectile<CustomAttactProj>("ProjectileModel");

            proj.transform.position = startpos;

            proj.Target = Controller.Target;

            proj.Speed = 1.0f;

            DataInfo.AttackCooldown.EnterCooling();
        }

        /// <summary>
        /// 使用指定弹道模型创建一个弹道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projModelName"></param>
        /// <returns></returns>
        public T CreateProjectile<T>(string projModelName)
            where T:Projectile
        {
            GameObject proj = new GameObject(name + "-Projectile");

            if (!string.IsNullOrEmpty(projModelName))
            {
                var go = Resources.Load<GameObject>("Projectile/" + projModelName);
                var temp = Instantiate(go, proj.transform, false);
                temp.name = projModelName;
            }

            return proj.AddComponent<T>();
        }
    }
}
