using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poi
{
    public class Cooldown:MaxDataProperty
    {
        public Cooldown(float time)
        {
            Max = time;
        }

        /// <summary>
        /// 完成冷却
        /// </summary>
        public void Refresh()
        {
            Current = 0f;
        }

        /// <summary>
        /// 进入冷却
        /// </summary>
        public void EnterCooling()
        {
            Current = Max;
        }
    }
}
