using System;
using BasicSkills;
using Enums;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class LevelBasicsManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private TextMeshProUGUI gameTitle;

        [SerializeField] 
        private GameObject countdownPrefab;
        [SerializeField] 
        private Timer timer;
        [SerializeField] 
        private TextMeshProUGUI scoreCountText;
        [SerializeField]
        private TextMeshProUGUI mistakeCountText;
        [SerializeField] 
        private GameObject mistakeVisualizerPrefab;
        [SerializeField] 
        private GameObject winPanel;
        [SerializeField] 
        private TextMeshProUGUI winText;
        [SerializeField] 
        private GameObject pausePanel;
        [SerializeField]
        public GameObject contentParent;
        
        public BasicSkill BasicSkill;

        private void Start()
        {
            NewGame();
        }

        #region Game Control

        public void NewGame()
        {
            var gameManager = GameManager.Singleton;
            ResetGame();
            if (gameManager.gameSettings.showCountdown)
            {
                var canvas = GameObject.Find("Canvas");
                var countdown = Instantiate(countdownPrefab, canvas.transform).GetComponent<Countdown>();
                countdown.Init(gameManager.gameSettings.countdownTime, StartGame);
            }
            else
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGameRunning = true;
            gameManager.isGamePaused = false;
            gameManager.BasicGame.IsRunning = true;
            
            timer.Init(isCountingUp: false, startingTime: gameManager.gameSettings.timeLimit,
                timerRunOutCallback: EndGame);
            BasicSkill.Init(this);
        }

        private void EndGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGameRunning = false;
            gameManager.isGamePaused = true;
            gameManager.BasicGame.IsRunning = false;
            
            winPanel.SetActive(true);
            winText.text = $"You scored {gameManager.BasicGame.Score} points in {gameManager.gameSettings.timeLimit} seconds!";
            timer.StopTimer();
        }

        public void PauseGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGamePaused = true;
            pausePanel.SetActive(true);
        }

        public void ResumeGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGamePaused = false;
            pausePanel.SetActive(false);
        }
        
        public void BackToMainMenu()
        {
            var gameManager = GameManager.Singleton;
            gameManager.LoadScene(Constants.MAIN_MENU_SCENE);
        }

        private void ResetGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGameRunning = false;
            gameManager.isGamePaused = false;
            gameManager.SortingGame.IsRunning = false;
            
            // reset score and mistakes
            gameManager.BasicGame.Score = 0;
            scoreCountText.text = "Score: 0";
            gameManager.BasicGame.Mistakes = 0;
            mistakeCountText.text = "Mistakes: 0";
            
            BasicSkill?.DestroyTask();
            var basicSkill = gameManager.BasicGame.BasicSkill;
            BasicSkill = basicSkill switch
            {
                EBasicSkill.IdentifySmallestElement => new IdentifySmallestElement(),
                EBasicSkill.IdentifyLargestElement => new IdentifyLargestElement(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            gameTitle.text = BasicSkill.GetTaskTitle();
            
            winPanel.SetActive(false);
            pausePanel.SetActive(false);
            
            timer.StopTimer();
            StopAllCoroutines();
        }

        #endregion

        #region Game Logic

        public void IncreaseScoreCount()
        {
            var gameManager = GameManager.Singleton;
            gameManager.BasicGame.Score++;
            scoreCountText.text = $"Score: {gameManager.BasicGame.Score}";
        }
        
        public void IncreaseMistakeCount()
        {
            var gameManager = GameManager.Singleton;
            gameManager.BasicGame.Mistakes++;
            mistakeCountText.text = $"Mistakes: {gameManager.BasicGame.Mistakes}";
            var canvas = GameObject.Find("Canvas");
            var mistakeVisualizer =
                Instantiate(mistakeVisualizerPrefab, canvas.transform).GetComponent<MistakeVisualizer>();
            mistakeVisualizer.Init(gameManager.gameSettings.errorCooldown);
        }

        #endregion
    }
}