using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class Pawn
    {
        protected DataInfo dataInfo;
        public void Init(DataInfo Info)
        {
            dataInfo = Info;
        }

        public PawnInfo DataInfo
        {
            get
            {
                return dataInfo as PawnInfo;
            }
        }
    }
}
