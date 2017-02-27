using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TargetPoint : MonoBehaviour {
    public bool isShowInGame = false;



    public Mesh PlayerStartMesh;
    // Use this for initialization
    public Color PlayerStartColor  = new Color32(105,227,116,41);

    private void OnDrawGizmos()
    {
        DrawPlayerStart();
    }

    private void DrawPlayerStart()
    {
        Gizmos.color = PlayerStartColor;
        Gizmos.DrawWireMesh(PlayerStartMesh, transform.position + Vector3.up, transform.rotation);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up,
            transform.position + Vector3.up + transform.forward * 1.5f);
    }
}
