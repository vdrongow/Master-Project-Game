using System.Collections;
using System.Collections.Generic;
using Adlete;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class LoginManager : MonoBehaviour
    {
        [Header("UI Elements for Login")]
        [SerializeField]
        private GameObject loginPanel;
        [SerializeField]
        private InputField usernameInputField;
        [SerializeField]
        private InputField roomCodeInputField;
        [SerializeField]
        private Button loginButton;
        [SerializeField]
        private TextMeshProUGUI infoText;
        [SerializeField]
        private Button playOfflineButton;
        
        [Header("UI Elements for Waiting Room")]
        [SerializeField]
        private GameObject waitingRoomPanel;
        [SerializeField]
        private TextMeshProUGUI lobbyNameText;
        [SerializeField]
        private TextMeshProUGUI roomCodeText;
        [SerializeField]
        private TextMeshProUGUI waitingRoomText;

        private const string UsernameKey = "Username";
        
        private float _roomUpdateTime;

        private void Awake()
        {
            loginPanel.SetActive(true);
            waitingRoomPanel.SetActive(false);
            
            if(PlayerPrefs.HasKey(UsernameKey))
            {
                usernameInputField.text = PlayerPrefs.GetString(UsernameKey);
            }
            loginButton.onClick.AddListener(CheckAndLogin);
            playOfflineButton.onClick.AddListener(() =>
            {
                GameManager.Singleton.LoadScene(Constants.MAIN_MENU_SCENE);
            });

            usernameInputField.onValueChanged.AddListener(_ => CheckLoginData());
            roomCodeInputField.onValueChanged.AddListener(_ => CheckLoginData());
            CheckLoginData();
        }
        
        private void Start()
        {
            var moduleConnection = ModuleConnection.Singleton;
            // check the status of the module connection
            moduleConnection.CheckStatus(
                info => Debug.Log($"StatusCode: {info.statusCode}, StatusMessage: {info.statusDescription}, TimeStamp: {info.timestamp}"),
                errorString => Debug.Log($"Error while checking status: {errorString}"),
                () => Debug.Log("CheckStatus finished"));
        
            // fetch service configuration
            moduleConnection.FetchServiceConfiguration(
                config => Debug.Log($"ServiceConfiguration: {string.Join(",", config.activityNames)}, {string.Join(",", config.initialScalarBeliefIds)}"),
                errorString => Debug.Log($"Error while fetching service configuration: {errorString}"),
                () => Debug.Log("FetchServiceConfiguration finished"));
        }

        private void CheckLoginData()
        {
            loginButton.interactable = !string.IsNullOrWhiteSpace(usernameInputField.text) &&
                                       !string.IsNullOrWhiteSpace(roomCodeInputField.text);
        }

        private void Login(string username)
        {
            var gameManager = GameManager.Singleton;
            var moduleConnection = ModuleConnection.Singleton;
            var learnerID = moduleConnection.GetLearnerIDFromUsername(username);
            moduleConnection.Login(learnerID,
                _ => Debug.Log($"Logged in with learnerID: {learnerID}"),
                errorString => Debug.Log($"Error while logging in: {errorString}"),
                () => gameManager.StartSession());
        }
    
        private void CreateNewLearner(string username)
        {
            var gameManager = GameManager.Singleton;
            var moduleConnection = ModuleConnection.Singleton;
            var learnerID = moduleConnection.GetLearnerIDFromUsername(username);
            moduleConnection.CreateNewLearnerAndLogin(learnerID, "level_beginner",
                _ => Debug.Log($"Created Learner with learnerID: {learnerID}"),
                errorString => Debug.Log($"Error while creating learner: {errorString}"),
                () => gameManager.StartSession());
        }

        private void CheckAndLogin()
        {
            var enteredUsername = usernameInputField.text;
            var enteredRoomCode = roomCodeInputField.text;
            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredRoomCode))
            {
                return;
            }
            var moduleConnection = ModuleConnection.Singleton;
        
            // check if the learner exists and login to adlete 
            var learnerID = moduleConnection.GetLearnerIDFromUsername(enteredUsername);
            moduleConnection.FetchLearner(learnerID, learner =>
                {
                    Debug.Log($"Learner: {learner}");
                    Login(enteredUsername);
                },
                errorString =>
                {
                    Debug.Log($"Error while fetching learner: {errorString}");
                    CreateNewLearner(enteredUsername);
                },
                () =>
                {
                    Debug.Log("FetchLearner finished");
                });
            
            JoinRoom(enteredUsername, enteredRoomCode);
        
            PlayerPrefs.SetString(UsernameKey, enteredUsername);
            PlayerPrefs.Save();
        }
        
        private async void JoinRoom(string playerName, string roomCode)
        {
            var gameManager = GameManager.Singleton;
            try
            {
                var options = new JoinLobbyByCodeOptions
                {
                    Player = new Player
                    {
                        Data = new Dictionary<string, PlayerDataObject>
                        {
                            {
                                Constants.PLAYER_NAME,
                                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)
                            },
                            {
                                Constants.PLAYER_FINISHED_LEVELS,
                                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, gameManager.player_finishedLevels.ToString())
                            },
                            {
                                Constants.PLAYER_TOTAL_MISTAKES,
                                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, gameManager.player_totalMistakes.ToString())
                            },
                            {
                                Constants.PLAYER_TOTAL_PLAYED_TIME,
                                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, gameManager.player_totalPlayedTime.ToString())
                            }
                        }
                    }
                };
                gameManager.CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(roomCode, options);
                gameManager.SubscribeToLobbyEvents();
                
                loginPanel.SetActive(false);
                waitingRoomPanel.SetActive(true);
                lobbyNameText.text = gameManager.CurrentLobby.Name;
                roomCodeText.text = gameManager.CurrentLobby.LobbyCode;
                waitingRoomText.text = "Waiting for the host to start the game...";
                
                // directly load scene if user joined late
                gameManager.CurrentLobby.Data.TryGetValue(Constants.LOBBY_IS_GAME_STARTED, out var started);
                var serverStartedGame = started != null && bool.Parse(started.Value);
                if(serverStartedGame)
                {
                    gameManager.LoadScene(Constants.MAIN_MENU_SCENE);
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                StartCoroutine(ShowInfoTextCoroutine(e.Reason.ToString()));
            }
        }

        private IEnumerator ShowInfoTextCoroutine(string text)
        {
            infoText.text = text;
            yield return new WaitForSeconds(3);
            infoText.text = "";
        }
    }
}