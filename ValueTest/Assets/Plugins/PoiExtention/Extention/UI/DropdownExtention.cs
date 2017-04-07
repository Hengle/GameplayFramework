using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.UI
{
    public static class DropdownExtention
    {
        public static Dropdown.OptionData SetValue(this Dropdown drop, string name)
        {
            Dropdown.OptionData res = null;
            for (int i = 0; i < drop.options.Count; i++)
            {
                if (drop.options[i].text == name)
                {
                    res = drop.options[i];
                    break;
                }
            }
            return res;
        }
    }
}
