using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class GM : MonoBehaviour {

    public static GM GMInstance;

    // 加载脚本实例时调用 Awake
    private void Awake()
    {
        GMInstance = this;
    }


    // Use this for initialization
    void Start () {
        LoadSceneAsync(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
