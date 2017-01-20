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

            Init();
        }

        public void Init()
        {
            InitTransform();
        }

        PawnController controller;
        public PawnController Controller { get { return controller; } set { controller = value; } }
        public AIController AIController { get { return controller as AIController; } set { controller = value; } }
        public PlayerController PlayerController { get { return controller as PlayerController; } set { controller = value; } }

        
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public PawnInfo DataInfo => dataInfo as PawnInfo;

        public void AlterPorperty<T>(PropertyType type, T changedValue)
        {

        }

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

        /// <summary>
        /// 刚体组件
        /// </summary>
        public Rigidbody Rigidbody { get; protected set; }

        /// <summary>
        /// 玩家当前状态
        /// </summary>
        public PawnState State { get; private set; } = PawnState.Idle;

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();


            ///初始模型中心点
            Chest = Animator.GetBoneTransform(HumanBodyBones.Chest);
            if (!Chest)
            {
                GameObject go = new GameObject("Chest");
                go.transform.SetParent(transform);
                go.transform.ResetLocal();
                go.transform.localPosition = new Vector3(0, DataInfo.Height / 2, 0);
            }
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            ///必须计算当前使用的命令FixedUpdate中必须最优先
            FixedUpdateTransform();
        }

        
    }
}
