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
    public partial class Pawn:ISkillTarget
    {
        #region ISkillTarget

        #endregion



        #region CastSkill

        public float AttackCooldown => DataInfo.AttackCooldown.Current;

        internal void Attack()
        {
            var startpos = transform.localToWorldMatrix.MultiplyPoint3x4(
                new Vector3(0, DataInfo.Height * 2 / 3, 0.5f));

            //string projName = "projectile";
            string projName = "proj2";
            CustomAttactProj proj = CreateProjectile<CustomAttactProj>(projName,startpos);

            proj.Owner = this;

            proj.lifeTime = 10f;

            proj.IsTracking = false;
            proj.Target = Controller.Target;
            proj.Speed = 0.8f;

            DataInfo.AttackCooldown.EnterCooling();
        }

        /// <summary>
        /// 使用指定弹道模型创建一个弹道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projModelName"></param>
        /// <param name="startPosition"></param>
        /// <param name="startEulerAngles"></param>
        /// <returns></returns>
        public T CreateProjectile<T>(string projModelName,Vector3 startPosition = default(Vector3),
            Vector3 startEulerAngles = default(Vector3))
            where T:Projectile
        {
            GameObject proj = new GameObject(name + "-Projectile");

            if (!string.IsNullOrEmpty(projModelName))
            {
                var go = Resources.Load<GameObject>("Projectile/" + projModelName);
                var temp = Instantiate(go, proj.transform, false);
                temp.name = projModelName;
            }

            proj.transform.position = startPosition;
            proj.transform.eulerAngles = startEulerAngles;

            return proj.AddComponent<T>();
        }

        /// <summary>
        /// 使用指定弹道模型创建一个弹道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projModelName"></param>
        /// <param name="start"></param>
        /// <param name="useRotation"></param>
        /// <returns></returns>
        public T CreateProjectile<T>(string projModelName,Transform start,bool useRotation = true)
            where T : Projectile
        {
            if (useRotation)
            {
                return CreateProjectile<T>(projModelName, start.position, start.eulerAngles);
            }
            else
            {
                return CreateProjectile<T>(projModelName, start.position);
            }
        }

        #endregion
    }
}
