using Adlete;
using Configs;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; } = null!;
    
    [HideInInspector]
    public GameState gameState = null!;
    
    [Header("Configs")]
    public GameSettings gameSettings = null!;
    public ArraySettings arraySettings = null!;
    
    [Header("Game State")]
    public bool isGameRunning;
    public bool isGamePaused;
    public int mistakeCount;

    public Game Game;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }
        else if(Singleton != this)
        {
            Destroy(gameObject);
        }
        
        gameState = GetComponent<GameState>();

        Game = new Game(gameSettings.defaultSortingAlgorithm, 
            gameSettings.defaultSortType,
            gameSettings.defaultArraySize);
    }

    public void StartSession()
    {
        var moduleConnection = ModuleConnection.Singleton;
        moduleConnection.StartSession(_ => Debug.Log("Session started"),
            errorString => Debug.Log($"Error while starting session: {errorString}"));
    }

    public void StopSession()
    {
        var moduleConnection = ModuleConnection.Singleton;
        moduleConnection.StopSession(_ => Debug.Log("Session stopped"),
            errorString => Debug.Log($"Error while stopping session: {errorString}"));
    }

    public void StartLevel(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
    {
        Game = new Game(sortingAlgorithm, sortType, arraySize);
        LoadScene(Constants.LEVEL_SCENE);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnApplicationQuit()
    {
        StopSession();
    }

    // TODO: check if this works with the callback from the index.html
    /*
     * window.onbeforeunload = () => {
        unityInstance.SendMessage(GameManager, CloseCallback);
        return "Are you sure to leave this page?";
      }
     */
    public void CloseCallback()
    {
        StopSession();
    }
}