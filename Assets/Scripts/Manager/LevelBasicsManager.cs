using TMPro;
using UnityEngine;

namespace Manager
{
    public class LevelBasicsManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI gameTitle;
        [SerializeField]
        private GameObject countdownPrefab;
        [SerializeField]
        private Timer timer;
        [SerializeField]
        private TextMeshProUGUI correctCountText;
        [SerializeField]
        private GameObject mistakeVisualizer;
        [SerializeField]
        private GameObject winPanel;
        [SerializeField]
        private TextMeshProUGUI winText;
        [SerializeField]
        private GameObject pausePanel;
    
        private void Start()
        {
            var gameManager = GameManager.Singleton;
            winPanel.SetActive(false);
            pausePanel.SetActive(false);
            mistakeVisualizer.SetActive(false);
            //gameTitle.text = gameManager.Game.SortingAlgorithm.ToString();
            if(gameManager.gameSettings.showCountdown)
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
            ResetScore();
            timer.Init(isCountingUp: false, startingTime: gameManager.gameSettings.timeLimit, timerRunOutCallback: EndGame);
        }
    
        private void EndGame()
        {
            var gameManager = GameManager.Singleton;
        
            gameManager.isGameRunning = false;
            gameManager.isGamePaused = true;
            gameManager.BasicGame.IsRunning = false;
            winPanel.SetActive(true);
            winText.text = $"You finished in {timer.GetTime()} with {gameManager.BasicGame.Score} correct answers!";
            timer.StopTimer();
        }

        private void ResetScore()
        {
            var gameManager = GameManager.Singleton;
            gameManager.BasicGame.Score = 0;
            correctCountText.text = "Correct: 0";
        }
    }
}