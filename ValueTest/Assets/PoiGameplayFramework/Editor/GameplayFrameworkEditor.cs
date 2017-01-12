using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Poi;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class GameplayFrameworkEditor
{
    [MenuItem("Assets/Create/GameMode", priority = 202)]
    static void MenuCreatePostProcessingProfile()
    {
        var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateGameMode>(), "New GameMode.asset", icon, null);
    }
}

internal class DoCreateGameMode : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        GameMode gamemode = ScriptableObject.CreateInstance<GameMode>();
        gamemode.name = Path.GetFileName(pathName);
        AssetDatabase.CreateAsset(gamemode, pathName);
        ProjectWindowUtil.ShowCreatedAsset(gamemode);
    }
}

[CustomEditor(typeof(Pawn))]
public class PawnEditor : ShowPropertyEditor<Pawn> { }
[CustomEditor(typeof(Character))]
public class CharacterEditor : ShowPropertyEditor<Character> { }
[CustomEditor(typeof(Player))]
public class PlayerEditor : ShowPropertyEditor<Player> { }
[CustomEditor(typeof(Monster))]
public class MonsterEditor : ShowPropertyEditor<Monster> { }