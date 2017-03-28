using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;

public partial class GM
{
    private GameObject CreatePawnGameObject()
    {
        return GameObject.Instantiate(GameMode.DefaultPawn);
    }

    public GameObject CreatePlayer()
    {
        return CreatePlayer(GameObject.FindWithTag(nameof(PoiTag.PlayerStart))?.transform);
    }

    private GameObject CreatePlayer(Transform startpos)
    {
        var go = CreatePawnGameObject();
        go.transform.Apply(startpos);
        go.transform.ApplyRotationY(startpos);
        return go;
    }
}
