﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

    public static GM GMInstance;

    //public static List<PlayerController> PlayerControllers { get; private set; }
    //    = new List<PlayerController>();

    public static List<Controller> Controllers { get; private set; }
        = new List<Controller>();

    // 加载脚本实例时调用 Awake
    private void Awake()
    {
        GMInstance = this;
    }


    // Use this for initialization
    void Start () {

        Wait(LoadSceneAsync(1),()=>
        {
            PlayerController.Instance.CreatePlayer();
        });
	}

    private void Wait(AsyncOperation asyncOperation, Action Callback)
    {
        StartCoroutine(Func(asyncOperation, Callback));
    }

    private IEnumerator Func(AsyncOperation asyncOperation, Action callback,float waitTime = -1)
    {
        if (asyncOperation != null)
        {
            while (!asyncOperation.isDone)
            {
                if (waitTime > 0)
                {
                    yield return new WaitForSeconds(waitTime);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        callback?.Invoke();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
