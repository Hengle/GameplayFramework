using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMsg : MonoBehaviour {

    [SerializeField]
    private Text msgText;
    public bool IsShow => Text?.enabled ?? false;
    public Text Text
    {
        get
        {
            return msgText;
        }
    }

    private void Awake()
    {
        UI.HelpMsg = this;
    }

    public void OnOff()
    {
        if (Text)
        {
            Text.enabled = !Text.enabled;
        }
    }
}
