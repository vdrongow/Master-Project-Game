using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class GameStateViewerWindow : EditorWindow
{
    [SerializeField]
    private string state = string.Empty;
    
    private const string WAITING_FOR_PLAY_MODE = "Waiting For PlayMode";
    
    [MenuItem("Custom/GameState Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<GameStateViewerWindow>();
        window.titleContent = new GUIContent("GameState Viewer");
        window.Show();
    }
    
    protected void OnEnable()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }
    
    private void PlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredPlayMode:
                Debug.Log("Entered Play Mode");
                break;
            case PlayModeStateChange.ExitingPlayMode:
                Debug.Log("Exiting Play Mode");
                break;
            case PlayModeStateChange.EnteredEditMode:
            case PlayModeStateChange.ExitingEditMode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("GameState Viewer", EditorStyles.boldLabel);
        DrawTextBox(state);
    }

    private void OnInspectorUpdate()
    {
        var gameManager = GameManager.Singleton;
        if (gameManager == null || gameManager.gameState == null)
        {
            return;
        }
        DumpGameStateOverview(gameManager.gameState);
        Repaint();
    }
    
    private static void DrawTextBox(string text)
    {
        var style = new GUIStyle(GUI.skin.window)
        {
            wordWrap = true,
            alignment = TextAnchor.UpperLeft,
            padding = new RectOffset(10, 10, 30, 30)
        };
        var shouldBeDisabled = Event.current.type != EventType.Repaint;
        using var disabledScope = new EditorGUI.DisabledScope(shouldBeDisabled);
        EditorGUILayout.TextArea(!EditorApplication.isPlaying && string.IsNullOrEmpty(text)
            ? WAITING_FOR_PLAY_MODE
            : text, 
            style);
    }

    private void DumpGameStateOverview(GameState gameState)
    {
        state = gameState.DumpGameState();
    }
}