using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour {

    public Camera cam;
    public GameObject cursorsprite;
    public GameObject MEshcursor;
    [Range(0,1f)]
    public float rotate = .1f;

	// Use this for initialization
	void Start () {
        

    }

    readonly Vector3 v = new Vector3(0, 0, 1);
	// Update is called once per frame
	void Update () {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        cursorsprite.transform.localPosition = pos + v;
        MEshcursor.transform.localPosition = pos + v;
        cursorsprite.transform.Rotate(new Vector3(0, 0, rotate), Space.Self);
	}
}
