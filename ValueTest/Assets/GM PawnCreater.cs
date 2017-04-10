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
    public static GameObject CreatePawnGameObject(string name = null, Transform startpos = null)
    {
        name = name ?? ModelDic.First().Key;
        var ori = Resources.Load<GameObject>(ModelDic[name].Path);
        var go = GameObject.Instantiate(ori);
        go.transform.Apply(startpos);
        go.transform.ApplyRotationY(startpos);
        return go;
    }

    public static GameObject CreatePawnGameObject(PawnInfo info, Transform startpos = null)
    {
        var name = info?.ModelName?? ModelDic.First().Key;
        var go = CreatePawnGameObject(name, startpos);

        go.name = $"{info.Name}--{go.name}";
        return go;
    }
}
