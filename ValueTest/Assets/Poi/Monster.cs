using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Poi
{
    /// <summary>
    /// 怪物
    /// </summary>
    public class Monster :Pawn
    {
        /// <summary>
        /// 怪物信息（数据模型）
        /// </summary>
        public new MonsterInfo DataInfo => dataInfo as MonsterInfo;

        [SerializeField]
        public Transform[] destinations = new Transform[3];

        public void Move()
        {
            var r = new System.Random();
            int a = r.Next(destinations.Length);
            agent?.SetDestination(destinations[a].position);
        }

        private NavMeshAgent agent;
        // 仅在首次调用 Update 方法之前调用 Start
        protected override void Start()
        {
            base.Start();

            var pos = GameObject.Find("Pos (3)");
            destinations[0] = pos.transform;
            var pos1 = GameObject.Find("Pos (1)");
            destinations[1] = pos1.transform;
            var pos2 = GameObject.Find("Pos (2)");
            destinations[2] = pos2.transform;


            agent = GetComponent<NavMeshAgent>();

            StartCoroutine(RandomMove());
        }

        private IEnumerator RandomMove()
        {
            while (true)
            {
                Move();
                yield return new WaitForSeconds(10);
            }
        }


        protected override void Update()
        {
            base.Update();

            Animator.SetFloat("Speed", agent.speed);
        }
    }
}
