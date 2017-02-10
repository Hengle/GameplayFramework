using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弹道
/// </summary>
public class Projectile : MonoBehaviour {

    public Target Target { get; set; }
    /// <summary>
    /// 是不是跟踪型弹道
    /// </summary>
    public bool IsTracking => Target is TransformTarget;

    public float Speed { get; internal set; } = 1.0f;

    Rigidbody Rig;
    

    // Use this for initialization
    void Start () {
        Rig = this.GetComponentIfNullAdd<Rigidbody>();
        Rig.useGravity = false;

        if (Target is PointTarget)
        {
            transform.LookAt(Target.TargetWorldPosotion);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Target is TransformTarget)
        {
            transform.LookAt(Target.TargetWorldPosotion);
        }
        transform.Translate(new Vector3(0,0,Speed),Space.Self);
	}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {

    }


}
