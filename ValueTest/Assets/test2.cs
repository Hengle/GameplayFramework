using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {

    public Vector2 Arrow = new Vector2(0, 1);
    public Vector3 t;
	// Use this for initialization
	void Start () {
        //StartCoroutine(Tiao());
        t = Vector3.MoveTowards(Vector3.zero, Vector3.up, 0.5f);
	}

    private IEnumerator Tiao()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("B");
    }

    // Update is called once per frame
    void Update () {
        var jiao = Vector2.Angle(Vector2.up, Arrow);
        Debug.Log(jiao);
	}
}
