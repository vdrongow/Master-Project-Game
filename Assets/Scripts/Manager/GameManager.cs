using Adlete;
using Configs;
using Enums;
using Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
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

        public SortingGame SortingGame;
        public BasicGame BasicGame;

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
            
            SortingGame = new SortingGame(gameSettings.defaultSortingAlgorithm, 
                gameSettings.defaultSortType,
                gameSettings.defaultArraySize);
            
            BasicGame = new BasicGame(gameSettings.defaultBasicConcept);
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

        public void StartSortingLevel(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
        {
            SortingGame = new SortingGame(sortingAlgorithm, sortType, arraySize);
            LoadScene(Constants.LEVEL_SORTING_SCENE);
        }
        
        public void StartBasicLevel(EBasicConcepts basicConcept)
        {
            BasicGame = new BasicGame(basicConcept);
            LoadScene(Constants.LEVEL_BASICS_SCENE);
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
}