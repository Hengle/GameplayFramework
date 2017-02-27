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

    private GameObject CreatePlayer()
    {
        var go = CreatePawnGameObject();
        var start = GameObject.Find($"{PoiTag.PlayerStart.ToString()}");
        go.transform.Apply(start?.transform);
        go.transform.ApplyRotationY(start?.transform);
        return go;
    }
}
