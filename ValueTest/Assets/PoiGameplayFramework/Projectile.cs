using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弹道
/// </summary>
public class Projectile : MonoBehaviour {

    public Target Target { get; set; }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0,0,0.1f),Space.Self);
	}
}
