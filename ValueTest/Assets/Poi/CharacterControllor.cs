using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class CharacterControllor
    {
        protected Pawn pawn;
        protected Pawn oldPawn;

        public Pawn Pawn => pawn;

        public void Possess(Pawn pawn)
        {
            oldPawn = this.pawn;
            this.pawn = pawn;
        }

        public void UnPossess()
        {

        }
    }
}
