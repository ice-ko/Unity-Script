using UnityEngine;
using System.Collections.Generic;
/*
using UnityEditor;
[InitializeOnLoad]
public class FullscreenPlayMode : MonoBehaviour {
    //The size of the toolbar above the game view, excluding the OS border.
    private const bool ENABLED = true;

    private static int tabHeight = 22;
     
    private static Dictionary<string, Vector2> settings = new Dictionary<string, Vector2> {
        //{"FF56", new Vector2(1920, 40)} // sharing your code? offsets go in here!
        {"FF56", new Vector2(0, 0)} // sharing your code? offsets go in here!
    };
     
    static FullscreenPlayMode() {
        if (ENABLED) {
            if (settings.ContainsKey(System.Environment.UserName)) {
                Debug.Log("####### FULLSCREENPLAYMODE");
                EditorApplication.playmodeStateChanged -= CheckPlayModeState;
                EditorApplication.playmodeStateChanged += CheckPlayModeState;
            }
        }
    }
     
    static void CheckPlayModeState() {
        // looks strange, but works much better!
        if (EditorApplication.isPlaying) {
            if (EditorApplication.isPlayingOrWillChangePlaymode) {
            FullScreenGameWindow();
            } else {
            CloseGameWindow();
            }
        }
    }
     
    static EditorWindow GetMainGameView() {
        EditorApplication.ExecuteMenuItem("Window/Game");
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null, null);
        return (EditorWindow)Res;
    }
     
    static Rect orig;
    static Vector2 min;
    static Vector2 max;
     
    static void FullScreenGameWindow() {
        EditorWindow gameView = GetMainGameView();
     
        Rect newPos = new Rect(0, 0 - tabHeight, Screen.currentResolution.width, Screen.currentResolution.height + tabHeight);
        newPos.position = newPos.position + settings[System.Environment.UserName];
        orig = gameView.position;
        min = gameView.minSize;
        max = gameView.maxSize;
     
        gameView.position = newPos;
        gameView.minSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height + tabHeight);
        gameView.maxSize = gameView.minSize;
        gameView.position = newPos;
     
    }
     
    static void CloseGameWindow() {
        EditorWindow gameView = GetMainGameView();
     
        gameView.position = orig;
        gameView.minSize = min;
        gameView.maxSize = max;
        gameView.position = orig;
    }
}
//*/