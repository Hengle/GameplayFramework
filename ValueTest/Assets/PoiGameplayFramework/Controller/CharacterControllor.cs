using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poi
{
    public class CharacterControllor:PawnController
    {
        public new Character Pawn => base.Pawn as Character;

    }
}
