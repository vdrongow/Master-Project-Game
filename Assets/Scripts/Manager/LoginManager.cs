using Adlete;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField usernameInputField;
        [SerializeField]
        private Button loginButton;

        private const string UsernameKey = "Username";

        private void Awake()
        {
            if(PlayerPrefs.HasKey(UsernameKey))
            {
                usernameInputField.text = PlayerPrefs.GetString(UsernameKey);
            }
            loginButton.onClick.AddListener(CheckAndLogin);

            usernameInputField.onValueChanged.AddListener(_ => CheckUsernameInput());
            CheckUsernameInput();
        }

        private void CheckUsernameInput()
        {
            loginButton.interactable = !string.IsNullOrWhiteSpace(usernameInputField.text);
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

        private void DeleteLearner(string username)
        {
            var moduleConnection = ModuleConnection.Singleton;
            var learnerID = moduleConnection.GetLearnerIDFromUsername(username);
            moduleConnection.DeleteLearner(learnerID,
                _ => Debug.Log($"Deleted Learner with learnerID: {learnerID}"),
                errorString => Debug.Log($"Error while deleting learner: {errorString}"));
        }

        private void CheckAndLogin()
        {
            var enteredUsername = usernameInputField.text;
            if (string.IsNullOrWhiteSpace(enteredUsername))
            {
                return;
            }
            var moduleConnection = ModuleConnection.Singleton;

            // check if the old learner exists
            var oldLearnerName = PlayerPrefs.GetString(UsernameKey);
            if(oldLearnerName != enteredUsername)
            {
                DeleteLearner(oldLearnerName);
            }
        
            // check if the learner exists
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
        
            PlayerPrefs.SetString(UsernameKey, enteredUsername);
            PlayerPrefs.Save();
        
            GameManager.Singleton.LoadScene(Constants.MAIN_MENU_SCENE);
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
    }
}