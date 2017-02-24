using System;
using System.Collections;
using System.Collections.Generic;
using Poi;
using UnityEngine;

/// <summary>
/// 弹道
/// </summary>
public class Projectile : MonoBehaviour {

    /// <summary>
    /// 弹道归属
    /// </summary>
    public Pawn Owner { get; internal set; }

    public ITarget Target { get; set; }
    /// <summary>
    /// 是不是跟踪型弹道
    /// </summary>
    public bool IsTracking { get; set; } = true;

    public float Speed { get; internal set; } = 1.0f;

    protected Rigidbody Rig;
    

    // Use this for initialization
    protected virtual void Start () {
        Rig = this.GetComponentIfNullAdd<Rigidbody>();
        Rig.useGravity = false;
	}
}

/// <summary>
/// 普通攻击弹道
/// </summary>
public class CustomAttactProj : Projectile
{
    protected override void Start()
    {
        base.Start();
        if (Target.First)
        {
            transform.LookAt(Target.First); 
        }
        else
        {
            transform.LookAt(Target.Point);
        }
    }

    void Update()
    {
        if (IsTracking)
        {
            if (Target.First)
            {
                transform.LookAt(Target.First);
            }
            else
            {
                transform.LookAt(Target.Point);
            }
        }        
        
        transform.Translate(new Vector3(0, 0, Speed), Space.Self);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "-----------"+ transform.position);

        Destroy(gameObject);
    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }

    protected virtual void OnTriggerStay(Collider other)
    {

    }
}
