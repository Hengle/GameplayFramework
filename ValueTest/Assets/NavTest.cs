using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {
    NavMeshAgent ag;
	// Use this for initialization
	void Start () {
        ag = GetComponent<NavMeshAgent>();
        ag.SetDestination(new Vector3(0, 0, 100));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
