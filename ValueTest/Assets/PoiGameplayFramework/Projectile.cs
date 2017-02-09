using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弹道
/// </summary>
public class Projectile : MonoBehaviour {

    public Target Target { get; set; }
    public float Speed { get; internal set; } = 1.0f;

    Rigidbody Rig;
    

    // Use this for initialization
    void Start () {
        Rig = this.GetComponentIfNullAdd<Rigidbody>();
        Rig.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0,0,Speed),Space.Self);
	}
}
