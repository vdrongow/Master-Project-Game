using System.Collections;
using Enums;
using SortingAlgorithms;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject arrayParent;
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI mistakeCountText;
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
        StartCoroutine(Countdown(gameManager.gameSettings.countdownTime));
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
                CreateArray(ESortingAlgorithm.BubbleSort, 5, ESortType.Unsorted);
                StartCoroutine(SortingAlgorithm.VisualizeSort());
            }
            // press P for playing the sorting algorithm
            if (Input.GetKeyDown(KeyCode.P))
            {
                // start sorting
                CreateArray(ESortingAlgorithm.BubbleSort, 5, ESortType.Unsorted);
                StartCoroutine(SortingAlgorithm.PlaySort());
            }
        }
    }

    public void BackToMainMenu()
    {
        var gameManager = GameManager.Singleton;
        gameManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }
    
    public void RestartLevel()
    {
        var gameManager = GameManager.Singleton;
        winPanel.SetActive(false);
        mistakeCountText.text = "Mistakes: 0";
        gameManager.mistakeCount = 0;
        timerText.text = "00:00";
        StartCoroutine(Countdown(gameManager.gameSettings.countdownTime));
    }
    
    private IEnumerator Countdown(int seconds)
    {
        var count = seconds;
        countdownText.text = "Ready?...";
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(true);
        while (count > 0) {
           
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count --;
        }
        countdownText.text = "GOOO!";
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);

        StartSorting();
    }
    
    private IEnumerator Timer()
    {
        var gameManager = GameManager.Singleton;
        var timer = 0;
        while(gameManager.isGameRunning)
        {
            // wait until the game is not paused
            yield return new WaitUntil(() => gameManager.isGamePaused == false);
            
            timer += 1;
            // show the timer in minutes and seconds
            var minutes = timer / 60;
            var seconds = timer % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
            yield return new WaitForSeconds(1);
        }
    }

    private void StartSorting()
    {
        var gameManager = GameManager.Singleton;
        var session = gameManager.Session;
        
        CreateArray(session.SortingAlgorithm, session.ArraySize, session.SortType);
        
        gameManager.isGameRunning = true;
        gameManager.isGamePaused = false;
        gameManager.mistakeCount = 0;
        mistakeCountText.text = "Mistakes: 0";
        StartCoroutine(Timer());
        StartCoroutine(SortingAlgorithm.PlaySort());
    }

    private void CreateArray(ESortingAlgorithm sortingAlgorithm, int arraySize, ESortType sortType)
    {
        var gameManager = GameManager.Singleton;
        var arraySettings = gameManager.arraySettings;
        
        StopAllCoroutines();
        
        ArrayView?.DestroyArray();
        ArrayView = new ArrayView(arrayParent, arraySize, arraySettings, sortType);
        
        SortingAlgorithm = sortingAlgorithm switch
        {
            ESortingAlgorithm.BubbleSort => new BubbleSort(),
            ESortingAlgorithm.SelectionSort => new BubbleSort(), // TODO: implement SelectionSort
            ESortingAlgorithm.InsertionSort => new BubbleSort(), // TODO: implement InsertionSort
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
        gameManager.isGameRunning = false;
        // TODO: gameManager.gameState.SaveHighscore(timerText.text);
        StopAllCoroutines();
        ArrayView?.DestroyArray();
        winPanel.SetActive(true);
        winText.text = $"You finished in {timerText.text} with {gameManager.mistakeCount} mistakes!";
    }
    
    public void AskForHelp()
    {
        Debug.Log("Ask for help");
    }
    
    public void IncreaseMistakeCount()
    {
        var gameManager = GameManager.Singleton;
        gameManager.mistakeCount++;
        mistakeCountText.text = $"Mistakes: {gameManager.mistakeCount}";
    }
}