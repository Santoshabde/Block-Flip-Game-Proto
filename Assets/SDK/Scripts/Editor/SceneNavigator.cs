using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Change variables and functions --  as per you game
/// </summary>
public class SceneNavigator : MonoBehaviour
{
    public const string INIT_SCENE_PATH = "Assets/HexaSort/Scenes/Init.unity";
    public const string GAME_SCENE_PATH = "Assets/HexaSort/Scenes/ProtoScene.unity";

    [MenuItem("SNGames/Scenes/OpenScene/Init")]
    [System.Obsolete]
    private static void NavigateToInitScene()
    {
        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.OpenScene(INIT_SCENE_PATH);
    }

    [MenuItem("SNGames/Scenes/OpenScene/GameScene")]
    [System.Obsolete]
    private static void NavigateToGameScene()
    {
        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        EditorApplication.OpenScene(GAME_SCENE_PATH);
    }

    [MenuItem("SNGames/Scenes/PlayGame _F12")]
    [System.Obsolete]
    private static void PlayGame()
    {
        if (EditorApplication.isPlaying)
            return;

        EditorApplication.OpenScene(INIT_SCENE_PATH);
        EditorApplication.isPlaying = true;
    }
}
