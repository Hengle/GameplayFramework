using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class GM
{
    MyCursor MyCursor;
    private void InitUI()
    {
        MyCursor = GameObject.Find("Canvas").GetComponentInChildren<MyCursor>();
        MyCursor.UseMyCursor();

        //Cursor.lockState = CursorLockMode.None;
        UI.HelpMsg.Text.text = helpMsg;
    }
}