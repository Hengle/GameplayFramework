using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Poi;

public class NavTest : MonoBehaviour {
    NavMeshAgent ag;
	// Use this for initialization
	void Start () {
        ag = GetComponent<NavMeshAgent>();
        ag.SetDestination(new Vector3(0, 0, 100));

    }
    CoolDown CD = new CoolDown(1);
	// Update is called once per frame
	void Update () {
        if (CD.Check(Time.deltaTime))
        {
            Debug.Log("冷却完成");
        }
        else
        {
            Debug.Log(CD.CoolDownTime);
        }
	}
}
