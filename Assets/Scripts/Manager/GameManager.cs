using System;
using System.Collections.Generic;
using Adlete;
using Configs;
using Enums;
using Structs;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable InconsistentNaming

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
        public bool serverGamePaused;
        
        [Header("Prefabs")]
        public GameObject serverPausedPanelPrefab = null!;

        public SortingGame SortingGame;
        public BasicGame BasicGame;
        
        public Lobby CurrentLobby;
        public string playerId;
        
        public int player_finishedLevels;
        public int player_totalMistakes;
        public int player_totalPlayedTime; // in seconds
        
        private GameObject _serverPausedPanel = null!;

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

        private async void Start()
        {
            if (CurrentLobby == null)
            {
                await UnityServices.InitializeAsync();
                AuthenticationService.Instance.SignedIn += () =>
                {
                    playerId = AuthenticationService.Instance.PlayerId;
                    Debug.Log("Signed in " + playerId);
                };
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
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

        /*public void SubmitFinishedSortingGame(ESortingAlgorithm sortingAlgorithm, int correctness, int playedTime, int mistakes)
        {
            var activityName = sortingAlgorithm switch
            {
                ESortingAlgorithm.BubbleSort => Constants.ACTIVITY_BUBBLE_SORT_FINISHED,
                ESortingAlgorithm.SelectionSort => Constants.ACTIVITY_SELECTION_SORT_FINISHED,
                ESortingAlgorithm.InsertionSort => Constants.ACTIVITY_INSERTION_SORT_FINISHED,
                _ => throw new ArgumentOutOfRangeException(nameof(sortingAlgorithm), sortingAlgorithm, null)
            };
            
            // TODO: add arraySize
            var additionalInfos = $"Played Time: {playedTime}, Mistakes: {mistakes}";
            SubmitActivityResult(activityName, correctness, additionalInfos);
        }*/

        public void SubmitActivityResult(string activityName, int correctness, string additionalInfos = "")
        {
            if(gameSettings.showDebugLogs)
            {
                Debug.Log($"SubmitActivityResult: {activityName}, {correctness}, {additionalInfos}");
            }
            var moduleConnection = ModuleConnection.Singleton;
            if (moduleConnection.GetLoggedInUser() == null)
            {
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

        #region Network Lobby

        public async void SubscribeToLobbyEvents()
        {
            var callbacks = new LobbyEventCallbacks();
            callbacks.DataChanged += OnLobbyDataChanged;
            try
            {
                await Lobbies.Instance.SubscribeToLobbyEventsAsync(CurrentLobby.Id, callbacks);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        
        private void OnLobbyDataChanged(Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> changedData)
        {
            if(changedData.TryGetValue(Constants.LOBBY_IS_GAME_STARTED, out var started))
            {
                var serverStartedGame = bool.Parse(started.Value.Value);
                if(serverStartedGame)
                {
                    LoadScene(Constants.MAIN_MENU_SCENE);
                }
            }
            if(changedData.TryGetValue(Constants.LOBBY_IS_GAME_PAUSED, out var paused))
            {
                var serverPausedGame = bool.Parse(paused.Value.Value);
                if(serverPausedGame)
                {
                    ServerPauseGame();
                }
                else
                {
                    ServerResumeGame();
                }
            }
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
        
        public void ServerPauseGame()
        {
            serverGamePaused = true;
            var canvas = FindObjectOfType<Canvas>();
            _serverPausedPanel = Instantiate(serverPausedPanelPrefab, canvas.transform);
        }
        
        public void ServerResumeGame()
        {
            serverGamePaused = false;
            if(_serverPausedPanel != null)
            {
                Destroy(_serverPausedPanel);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public async void IncreasePlayerFinishedLevels()
        {
            player_finishedLevels++;
            if (CurrentLobby == null)
            {
                return;
            }

            try
            {
                var updateOptions = new UpdatePlayerOptions()
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {
                            Constants.PLAYER_FINISHED_LEVELS,
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,
                                player_finishedLevels.ToString())
                        }
                    }
                };
                CurrentLobby = await LobbyService.Instance.UpdatePlayerAsync(CurrentLobby.Id, playerId, updateOptions);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void IncreasePlayerTotalMistakes()
        {
            player_totalMistakes++;
            if (CurrentLobby == null)
            {
                return;
            }

            try
            {
                var updateOptions = new UpdatePlayerOptions()
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {
                            Constants.PLAYER_TOTAL_MISTAKES,
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,
                                player_totalMistakes.ToString())
                        }
                    }
                };
                CurrentLobby = await LobbyService.Instance.UpdatePlayerAsync(CurrentLobby.Id, playerId, updateOptions);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void IncreasePlayerTotalPlayedTime(int seconds)
        {
            player_totalPlayedTime += seconds;
            if (CurrentLobby == null)
            {
                return;
            }

            try
            {
                var updateOptions = new UpdatePlayerOptions()
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {
                            Constants.PLAYER_TOTAL_PLAYED_TIME,
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,
                                player_totalPlayedTime.ToString())
                        }
                    }
                };
                CurrentLobby = await LobbyService.Instance.UpdatePlayerAsync(CurrentLobby.Id, playerId, updateOptions);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        
        public async void CloseGame()
        {
            var moduleConnection = ModuleConnection.Singleton;
            if(moduleConnection.GetLoggedInUser() != null)
            {
                StopSession();
                moduleConnection.Logout();
            }
            if(CurrentLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, playerId);
                    CurrentLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
            LoadScene(Constants.LOGIN_SCENE);
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