using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    /// <summary>
    /// 网络端Pawn控制器，用来在解析网络远端指令
    /// </summary>
    public class NetPawnController : PawnController
    {
        public double lastTransTime { get; private set; }
        public Trans LastTrans { get; private set; }
        Stack<InputCMD> CMD = new Stack<InputCMD>();

        internal void SetTrans(double serverTime, Trans trans)
        {
            lastTransTime = serverTime;
            this.LastTrans = trans;
        }

        internal void SetCMD(List<InputCMD> transList)
        {
            if (transList != null)
            {
                var collection = transList.OrderBy(iterator => iterator.ServerTime);
                foreach (var item in collection)
                {
                    CMD.Push(item);
                }
            }
        }

        protected override void Update()
        {
            if (!Pawn) return;

            InputCMD cmd = new InputCMD();
            InputCMD lastcmd = new InputCMD();
            while (CMD.Count > 0)
            {
                lastcmd = CMD.Pop();
                if (lastcmd.Jump)
                {
                    cmd.Jump = true;
                }
                if (lastcmd.IsAttact)
                {
                    cmd.IsAttact = true;
                }
            }

            cmd.Acceleration = lastcmd.Acceleration;
            cmd.NextAngle = lastcmd.NextAngle;

            ParseInputCommand(cmd);
            

            if (LastTrans != null)
            {
                //Pawn.Move(LastTrans);
            }
        }
    }
}
