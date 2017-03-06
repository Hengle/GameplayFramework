﻿using System.Collections;
using System.Collections.Generic;
using Poi;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

    [SerializeField]
    private GameObject onoff;
    [SerializeField]
    private InputField nameInput;

    private void Awake()
    {
        UI.Setting = this;
        onoff.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onoff.SetActive(!onoff.activeSelf);
        }
	}

    public void SetName()
    {
        if (nameInput && !string.IsNullOrEmpty(nameInput.text))
        {
            Player.SetName(nameInput.text);
            UI.NameLabel.Reset();
        }
    }
}