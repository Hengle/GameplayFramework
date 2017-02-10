using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour {

    public Camera cam;
	// Use this for initialization
	void Start () {
		
	}

    readonly Vector3 v = new Vector3(0, 0, 1);
	// Update is called once per frame
	void Update () {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = pos + v;
	}
}
