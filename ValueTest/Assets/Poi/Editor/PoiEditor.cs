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
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
    }

    private static void OnPlaymodeStateChanged()
    {
        if (EditorApplication.isPlaying)
        {
            PlayerPrefs.SetInt(PoiIsAlreadyPlayKey, 1);
        }
        else
        {
            var path = PlayerPrefs.GetString(PoiCurrentSceneKey, "");

            int isPlayed = PlayerPrefs.GetInt(PoiIsAlreadyPlayKey, 0);
            ///结束游戏回到之前编辑的场景
            
            if (isPlayed == 1)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    try
                    {
                        EditorSceneManager.OpenScene(path);

                    }
                    catch (System.Exception)
                    {

                    }
                }

                PlayerPrefs.SetInt(PoiIsAlreadyPlayKey, 0);
                PlayerPrefs.SetString(PoiCurrentSceneKey, "");
            }

        }
    }

    [MenuItem("Poi      /SaveAndPlayMainScene")]
    private static void SaveAndPlayMainScene()
    {
        Scene cur = EditorSceneManager.GetActiveScene();

        PlayerPrefs.SetString(PoiCurrentSceneKey, cur.path);

        //PlayerPrefs.SetInt(PoiIsCommandPlayKey, 1);

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

    const string PoiCurrentSceneKey  = "PoiKey0x0001";
    const string PoiIsCommandPlayKey = "PoiKey0x0002";
    const string PoiIsAlreadyPlayKey = "PoiKey0x0003";
}
