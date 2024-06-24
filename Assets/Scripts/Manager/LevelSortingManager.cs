using Enums;
using SortingAlgorithms;
using TMPro;
using UnityEngine;

namespace Manager
{
    public class LevelSortingManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject arrayParent;
        [SerializeField]
        private TextMeshProUGUI algorithmTitle;
        [SerializeField]
        private GameObject countdownPrefab;
        [SerializeField]
        private Timer timer;
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

        public ArrayView ArrayView;
        public SortingAlgorithm SortingAlgorithm;

        private void Start()
        {
            NewGame();
        }
        
        private void Update()
        {
            var gameManager = GameManager.Singleton;
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                BackToMainMenu();
            }

            if(gameManager.gameSettings.devMode)
            {
                // press V for visualizing the sorting algorithm
                if (Input.GetKeyDown(KeyCode.V))
                {
                    // start sorting algorithm visualization
                    ResetGame();
                    StartCoroutine(SortingAlgorithm.VisualizeSort());
                }
                // press P for playing the sorting algorithm
                if (Input.GetKeyDown(KeyCode.P))
                {
                    NewGame();
                }
            }
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
            gameManager.SortingGame.IsRunning = true;
            
            timer.Init();
            StartCoroutine(SortingAlgorithm.PlaySort());
        }
        
        public void EndGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGameRunning = false;
            gameManager.isGamePaused = true;
            gameManager.SortingGame.IsRunning = false;
            
            winPanel.SetActive(true);
            winText.text = $"You finished in {timer.GetTime()} with {gameManager.SortingGame.MistakeCount} mistakes!";
            // TODO: gameManager.gameState.SaveHighscore(timerText.text);
            // TODO: send the highscore to the server -> maybe something like finishedSorting with 1 for sorted and 0 for canceled game + additional info like time, mistakes, etc.
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
            
            gameManager.SortingGame.MistakeCount = 0;
            mistakeCountText.text = "Mistakes: 0";
            
            var sortingGame = gameManager.SortingGame;
            var arraySettings = gameManager.arraySettings;
        
            ArrayView?.DestroyArray();
            ArrayView = new ArrayView(arrayParent, sortingGame.ArraySize, arraySettings, sortingGame.SortType);
        
            SortingAlgorithm = sortingGame.SortingAlgorithm switch
            {
                ESortingAlgorithm.BubbleSort => new BubbleSort(),
                ESortingAlgorithm.SelectionSort => new SelectionSort(),
                ESortingAlgorithm.InsertionSort => new InsertionSort(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(sortingGame.SortingAlgorithm), sortingGame.SortingAlgorithm, null)
            };
        
            SortingAlgorithm.Init(this, ArrayView, arraySettings);
            
            algorithmTitle.text = gameManager.SortingGame.SortingAlgorithm.ToString();
            
            winPanel.SetActive(false);
            pausePanel.SetActive(false);
            
            timer.StopTimer();
            StopAllCoroutines();
        }

        #endregion
        
        #region Game Logic

        public void AskForHelp()
        {
            Debug.Log("Ask for help");
        }
    
        public void IncreaseMistakeCount()
        {
            var gameManager = GameManager.Singleton;
            gameManager.SortingGame.MistakeCount++;
            mistakeCountText.text = $"Mistakes: {gameManager.SortingGame.MistakeCount}";
            var canvas = GameObject.Find("Canvas");
            var mistakeVisualizer = Instantiate(mistakeVisualizerPrefab, canvas.transform).GetComponent<MistakeVisualizer>();
            mistakeVisualizer.Init(gameManager.gameSettings.errorCooldown);
        }
        
        #endregion
    }
}