using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

public static partial class PoiEditor
{
    [InitializeOnLoadMethod]
    private static void InitializeInEditor()
    {

        //EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
    }

    private static void OnPlaymodeStateChanged()
    {
        Debug.Log(PoiContext.Context.EditScene);
       
        if (!EditorApplication.isPlaying)
        {
            ///结束游戏回到之前编辑的场景
            if (string.IsNullOrEmpty(PoiContext.Context.EditScene))
            {
                try
                {
                    EditorSceneManager.OpenScene(PoiContext.Context.EditScene);

                }
                catch (System.Exception)
                {

                }
            }
        }
    }

    [MenuItem("Poi/SaveAndPlayMainScene")]
    private static void SaveAndPlayMainScene()
    {
        Scene cur = EditorSceneManager.GetActiveScene();

        EditorSceneManager.SaveOpenScenes();
        try
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        }
        catch (System.Exception)
        {

        }
        EditorApplication.isPlaying = true;
    }


    public class PoiContext: SynchronizationContext
    {
        public static readonly PoiContext Context = new PoiContext();
        public string EditScene ;
    }
}
