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
    public static GameObject CreatePawnGameObject(string name, Transform startpos = null)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        var ori = Resources.Load<GameObject>(GetModelPath(name));
        var go = GameObject.Instantiate(ori);
        go.transform.Apply(startpos);
        go.transform.ApplyRotationY(startpos);
        return go;
    }

    public static GameObject CreatePawnGameObject(PawnInfo info, Transform startpos = null)
    {
        var go = CreatePawnGameObject(info.ModelName, startpos);

        go.name = $"{info.Name}--{go.name}";
        return go;
    }
}
