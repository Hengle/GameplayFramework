using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TargetPoint : MonoBehaviour {
    public Mesh PlayerStartMesh;
    // Use this for initialization
    public Color PlayerStartColor  = Color.HSVToRGB(125,137,277);
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, 1);
        //Gizmos.DrawWireMesh(Mesh.)
        Gizmos.color = PlayerStartColor;
        Gizmos.DrawWireMesh(PlayerStartMesh,transform.position + Vector3.up,transform.rotation);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up,
            transform.position + Vector3.up + transform.forward * 1.5f);
    }


}
