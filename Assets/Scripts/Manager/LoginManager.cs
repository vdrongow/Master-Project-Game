using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adlete;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
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
        
        private Lobby _currentLobby;
        private string _playerId;
        
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

            usernameInputField.onValueChanged.AddListener(_ => CheckLoginData());
            roomCodeInputField.onValueChanged.AddListener(_ => CheckLoginData());
            CheckLoginData();
        }
        
        private async void Start()
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

            if (_currentLobby == null)
            {
                await UnityServices.InitializeAsync();
                AuthenticationService.Instance.SignedIn += () =>
                {
                    _playerId = AuthenticationService.Instance.PlayerId;
                    Debug.Log("Signed in " + _playerId);
                };
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        private void Update()
        {
            HandleRoomUpdate();
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

        private async void CheckAndLogin()
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
            
            // check for a lobby with the entered room code
            try
            {
                var options = new JoinLobbyByCodeOptions { Player = GetPlayer(enteredUsername) };
                _currentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(enteredRoomCode, options);
                loginPanel.SetActive(false);
                waitingRoomPanel.SetActive(true);
                lobbyNameText.text = _currentLobby.Name;
                roomCodeText.text = _currentLobby.LobbyCode;
                waitingRoomText.text = "Waiting for the host to start the game...";
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                StartCoroutine(ShowInfoTextCoroutine(e.Reason.ToString()));
            }
        
            PlayerPrefs.SetString(UsernameKey, enteredUsername);
            PlayerPrefs.Save();
        
            //GameManager.Singleton.LoadScene(Constants.MAIN_MENU_SCENE);
        }

        private async void HandleRoomUpdate()
        {
            if (_currentLobby == null || IsGameStarted())
            {
                return;
            }

            _roomUpdateTime -= Time.deltaTime;
            if (_roomUpdateTime <= 0)
            {
                _roomUpdateTime = 2f;
                try
                {
                    _currentLobby = await LobbyService.Instance.GetLobbyAsync(_currentLobby.Id);
                    if (IsinLobby())
                    {
                        if (IsGameStarted())
                        {
                            waitingRoomText.text = "Game is starting...";
                            GameManager.Singleton.LoadScene(Constants.MAIN_MENU_SCENE);
                        }
                    }
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                    if (e.Reason is LobbyExceptionReason.Forbidden or LobbyExceptionReason.LobbyNotFound)
                    {
                        _currentLobby = null;
                    }
                }
            }
        }

        private Player GetPlayer(string playerName)
        {
            var player = new Player
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            };

            return player;
        }

        private IEnumerator ShowInfoTextCoroutine(string text)
        {
            infoText.text = text;
            yield return new WaitForSeconds(3);
            infoText.text = "";
        }
        
        private bool IsHost() => _currentLobby != null && _currentLobby.HostId == _playerId;
    
        private bool IsinLobby()
        {
            if (_currentLobby.Players.Any(player => player.Id == _playerId))
            {
                return true;
            }

            _currentLobby = null;
            return false;
        }
    
        private bool IsGameStarted()
        {
            if (_currentLobby == null)
            {
                return false;
            }
            return _currentLobby.Data["IsGameStarted"].Value == "true";
        }

        private async void OnDestroy()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_currentLobby.Id, _playerId);
                _currentLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}