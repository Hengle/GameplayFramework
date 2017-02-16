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
    public partial class Pawn
    {

        // 当呈现器在任何照相机上都不可见时调用 OnBecameInvisible
        private void OnBecameInvisible()
        {
            UI.RemovePawn(this);
        }

        // 当呈现器在任何照相机上可见时调用 OnBecameVisible
        private void OnBecameVisible()
        {
            UI.AddPawn(this);
        }
    }
}
