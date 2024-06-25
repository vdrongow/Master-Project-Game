using System;
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
            
            BasicGame = new BasicGame(gameSettings.defaultBasicSkill);
        }

        #region Adlete
        
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
        
        public void SubmitFinishedSortingGame(ESortingAlgorithm sortingAlgorithm, int correctness, int playedTime, int mistakes)
        {
            var activityName = sortingAlgorithm switch
            {
                ESortingAlgorithm.BubbleSort => Constants.ACTIVITY_BUBBLE_SORT_FINISHED,
                ESortingAlgorithm.SelectionSort => Constants.ACTIVITY_SELECTION_SORT_FINISHED,
                ESortingAlgorithm.InsertionSort => Constants.ACTIVITY_INSERTION_SORT_FINISHED,
                _ => throw new ArgumentOutOfRangeException(nameof(sortingAlgorithm), sortingAlgorithm, null)
            };
            
            var additionalInfos = $"Played Time: {playedTime}, Mistakes: {mistakes}";
            SubmitActivityResult(activityName, correctness, additionalInfos);
        }

        public void SubmitActivityResult(string activityName, int correctness, string additionalInfos = "")
        {
            if(gameSettings.showDebugLogs)
            {
                Debug.Log($"SubmitActivityResult: {activityName}, {correctness}, {additionalInfos}");
            }
            var moduleConnection = ModuleConnection.Singleton;
            if (moduleConnection.GetLoggedInUser() == null)
            {
                Debug.LogWarning("User is not logged in. Cannot submit activity result.");
                return;
            }
            var observation = new Observation
            {
                activityName = activityName,
                activityCorrectness = correctness,
                activityDifficulty = 0.5f,
                timestamp = DateTime.Now,
                additionalInfos = additionalInfos
            };
            moduleConnection.SubmitActivityResult(observation);
        }

        #endregion

        public void StartSortingLevel(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
        {
            SortingGame = new SortingGame(sortingAlgorithm, sortType, arraySize);
            LoadScene(Constants.LEVEL_SORTING_SCENE);
        }
        
        public void StartBasicLevel(EBasicSkill basicSkill)
        {
            BasicGame = new BasicGame(basicSkill);
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