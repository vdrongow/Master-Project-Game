using Configs;
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
    
    private int sceneIndex;

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
    }
    
    private void Start()
    {
        sceneIndex = 0;
    }
    
    public void LoadNextScene()
    {
        sceneIndex++;
        if(sceneIndex >= gameSettings.sceneNames.Length)
        {
            sceneIndex = 0;
        }
        
        var sceneName = gameSettings.sceneNames[sceneIndex];
        SceneManager.LoadScene(sceneName);
    }
}