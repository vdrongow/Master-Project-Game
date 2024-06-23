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
            // delete all childs of the array parent
            foreach (Transform child in arrayParent.transform)
            {
                Destroy(child.gameObject);
            }

            var gameManager = GameManager.Singleton;
            winPanel.SetActive(false);
            pausePanel.SetActive(false);
            algorithmTitle.text = gameManager.SortingGame.SortingAlgorithm.ToString();
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
                    // start sorting
                    var sortingGame = gameManager.SortingGame;
                    CreateArray(sortingGame.SortingAlgorithm, sortingGame.ArraySize, sortingGame.SortType);
                    StartCoroutine(SortingAlgorithm.VisualizeSort());
                }
                // press P for playing the sorting algorithm
                if (Input.GetKeyDown(KeyCode.P))
                {
                    RestartLevel();
                }
            }
        }

        public void BackToMainMenu()
        {
            DestroyGame();
            var gameManager = GameManager.Singleton;
            gameManager.LoadScene(Constants.MAIN_MENU_SCENE);
        }
    
        public void RestartLevel()
        {
            var gameManager = GameManager.Singleton;
            DestroyGame();
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

        public void ShowMistakeVisualizer(float seconds)
        {
            var canvas = GameObject.Find("Canvas");
            var mistakeVisualizer = Instantiate(mistakeVisualizerPrefab, canvas.transform).GetComponent<MistakeVisualizer>();
            mistakeVisualizer.Init(seconds);
        }

        private void StartGame()
        {
            var gameManager = GameManager.Singleton;
            var sortingGame = gameManager.SortingGame;
        
            CreateArray(sortingGame.SortingAlgorithm, sortingGame.ArraySize, sortingGame.SortType);
        
            gameManager.isGameRunning = true;
            gameManager.isGamePaused = false;
            gameManager.SortingGame.IsRunning = true;
            
            timer.Init();
            StartCoroutine(SortingAlgorithm.PlaySort());
        }

        private void CreateArray(ESortingAlgorithm sortingAlgorithm, int arraySize, ESortType sortType)
        {
            var gameManager = GameManager.Singleton;
            var arraySettings = gameManager.arraySettings;
        
            DestroyGame();
        
            ArrayView = new ArrayView(arrayParent, arraySize, arraySettings, sortType);
        
            SortingAlgorithm = sortingAlgorithm switch
            {
                ESortingAlgorithm.BubbleSort => new BubbleSort(),
                ESortingAlgorithm.SelectionSort => new SelectionSort(),
                ESortingAlgorithm.InsertionSort => new InsertionSort(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(sortingAlgorithm), sortingAlgorithm, null)
            };
        
            SortingAlgorithm.Init(this, ArrayView, GameManager.Singleton.arraySettings);
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
    
        public void FinishSorting()
        {
            var gameManager = GameManager.Singleton;
            winPanel.SetActive(true);
            winText.text = $"You finished in {timer.GetTime()} with {gameManager.SortingGame.MistakeCount} mistakes!";
            DestroyGame();
            // TODO: gameManager.gameState.SaveHighscore(timerText.text);
            // TODO: send the highscore to the server -> maybe something like finishedSorting with 1 for sorted and 0 for canceled game + additional info like time, mistakes, etc.
        }
    
        public void AskForHelp()
        {
            Debug.Log("Ask for help");
        }
    
        public void IncreaseMistakeCount()
        {
            var gameManager = GameManager.Singleton;
            gameManager.SortingGame.MistakeCount++;
            mistakeCountText.text = $"Mistakes: {gameManager.SortingGame.MistakeCount}";
        }
    
        public void DestroyGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.isGameRunning = true;
            gameManager.isGamePaused = false;
            gameManager.SortingGame.IsRunning = false;
            winPanel.SetActive(false);
            ResetMistakes();
            timer.StopTimer();
            StopAllCoroutines();
            ArrayView?.DestroyArray();
        }

        private void ResetMistakes()
        {
            var gameManager = GameManager.Singleton;
            gameManager.SortingGame.MistakeCount = 0;
            mistakeCountText.text = "Mistakes: 0";
        }
    }
}