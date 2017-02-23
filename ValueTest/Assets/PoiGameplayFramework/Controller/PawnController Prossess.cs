using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public enum PossessType
    {
        Ctrl,
        OnlyLook,
    }

    /// <summary>
    /// 角色控制器
    /// </summary>
    public partial class PawnController
    {
        public bool IsFollowPawn { get; set; }

        /// <summary>
        ///控制模式
        /// </summary>
        public ControlMode Mode { get; set; }


        protected Pawn pawn;
        protected Pawn oldPawn;

        public Pawn Pawn => pawn;
        public Pawn OldPawn => oldPawn;

        public PossessType PossessType { get; protected set; }

        /// <summary>
        /// 控制角色
        /// </summary>
        /// <param name="pawn"></param>
        public virtual bool Possess(Pawn pawn, PossessType type = PossessType.Ctrl)
        {
            ///贪婪控制器处理
            if (pawn?.Controller?.Mode == ControlMode.Greedy && PossessType == PossessType.Ctrl)
            {
                return false;
            }

            ///前控制器释放
            pawn?.Controller?.UnPossess();

            oldPawn = this.pawn;
            this.pawn = pawn;

            pawn.Controller = this;

            if (IsFollowPawn)
            {
                ///控制器跟随
                transform.SetParent(pawn.EyeCamaraPos);

                transform.ResetLocal();
                
            }

            return true;
        }

        /// <summary>
        /// 释放角色
        /// </summary>
        /// <returns></returns>
        public Pawn UnPossess()
        {
            if (transform.parent = Pawn.transform)
            {
                transform.SetParent(null);
            }


            pawn.Controller = null;
            oldPawn = Pawn;
            pawn = null;
            return OldPawn;
        }
    }
}
