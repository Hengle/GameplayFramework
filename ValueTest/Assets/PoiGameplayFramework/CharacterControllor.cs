﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class CharacterControllor:PawnController
    {
        public Character Character => pawn as Character; 
    }
}