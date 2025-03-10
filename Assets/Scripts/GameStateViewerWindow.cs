﻿using System;
using Manager;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
public sealed class GameStateViewerWindow : EditorWindow
{
    [SerializeField]
    private string state = string.Empty;
    
    private const string WAITING_FOR_PLAY_MODE = "Waiting For PlayMode";

    private Vector2 scrollPosition;

    [MenuItem("Custom/GameState Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<GameStateViewerWindow>();
        window.titleContent = new GUIContent("GameState Viewer");
        window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }
    
    private void PlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredPlayMode:
                break;
            case PlayModeStateChange.ExitingPlayMode:
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
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 20));
        
        DrawTextBox(state);
        
        EditorGUILayout.EndScrollView();
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
#endif
