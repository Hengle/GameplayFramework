using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Pawn
    {
        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        protected DataInfo dataInfo;
        /// <summary>
        /// 通过数据模型初始化
        /// </summary>
        /// <param name="Info"></param>
        public void Init(DataInfo Info)
        {
            dataInfo = Info;
        }

        /// <summary>
        /// 角色信息（数据模型）
        /// </summary>
        public PawnInfo DataInfo => dataInfo as PawnInfo;
        
    }
}
