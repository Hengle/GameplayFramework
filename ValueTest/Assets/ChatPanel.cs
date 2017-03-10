using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour,IMonoBehaviour,IShow
{
    public bool IsShow => onoff?.activeSelf ?? false;

    [SerializeField]
    private GameObject onoff;

    public Text TextArea;
    public InputField Input;
    public Button Emoji;

    public float time = 10f;
    // Use this for initialization
    void Start () {
        UI.ChatPanel = this;
        onoff.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (time<=0)
        {
            onoff.SetActive(false);
            time = 10f;
        }
        else if(IsShow)
        {
            time -= Time.deltaTime;
        }
	}

    public void Commit()
    {

    }

    public void OnOff()
    {
        onoff.SetActive(!onoff.activeSelf);
    }

    internal void Show()
    {
        onoff.SetActive(true);
    }
}
