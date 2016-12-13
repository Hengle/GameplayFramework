using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Tiao());

	}

    private IEnumerator Tiao()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("B");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
