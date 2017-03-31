using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using Poi;
using UnityEngine.SceneManagement;
using System.Linq;

public partial class GM
{
    private GameObject CreatePawnGameObject(string name = null)
    {
        name = name ?? ModelDic.First().Key;
        var go = Resources.Load<GameObject>(ModelDic[name].Path);
        return GameObject.Instantiate(go);
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
