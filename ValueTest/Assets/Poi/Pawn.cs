using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色
    /// </summary>
    public partial class Pawn : MonoBehaviour
    {
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        protected DataInfo dataInfo;
        /// <summary>
        /// 通过数据模型初始化 
        /// <para>Awake紧随AddComponet执行,Init无法在Awake前执行，但可以在Start前执行</para>
        /// </summary>
        /// <param name="info"></param>
        public void Init(DataInfo info)
        {
            dataInfo = info;
        }

        public PawnController Controller { get; set; }

        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public PawnInfo DataInfo => dataInfo as PawnInfo;


        public virtual void OnHit(double damage)
        {
            DataInfo?.HP?.OnHit(damage);
            CheckDead();
        }

        public virtual void AddHP(double addValue)
        {
            DataInfo?.HP?.AddHP(addValue);
        }

        /// <summary>
        /// 检查死亡
        /// </summary>
        private void CheckDead()
        {
            if (DataInfo.IsDead)
            {
                OnDead?.Invoke();
            }
        }

        public void AlterPorperty<T>(PropertyType type, T changedValue)
        {

        }

        public event Action OnDead;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltatime"></param>
        public void TickDataInfo(float deltatime)
        {
            foreach (var item in DataInfo.RestoreProperties)
            {
                item.TickRestore(deltatime);
            }
        }

        protected virtual void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
        }


    }
}
