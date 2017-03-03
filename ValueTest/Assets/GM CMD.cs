using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class GM
{
    public void CMD(string cmdtext)
    {
        if (cmdtext == "clear")
        {
            CommandTool.Clear();
            return;
        }
    }
}