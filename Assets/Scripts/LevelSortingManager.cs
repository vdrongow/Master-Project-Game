using System;
using System.Collections;
using Enums;
using SortingAlgorithms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject mistakeVisualizer;
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
        mistakeVisualizer.SetActive(false);
        algorithmTitle.text = gameManager.Game.SortingAlgorithm.ToString();
        if(gameManager.gameSettings.showCountdown)
        {
            var canvas = GameObject.Find("Canvas");
            var countdown = Instantiate(countdownPrefab, canvas.transform).GetComponent<Countdown>();
            countdown.Init(gameManager.gameSettings.countdownTime, StartSorting);
        }
        else
        {
            StartSorting();
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
                CreateArray(gameManager.Game.SortingAlgorithm, gameManager.Game.ArraySize, gameManager.Game.SortType);
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
            countdown.Init(gameManager.gameSettings.countdownTime, StartSorting);
        }
        else
        {
            StartSorting();
        }
    }

    public void ShowMistakeVisualizer(float seconds)
    {
        StartCoroutine(MistakeVisualizerCoroutine(seconds));
    }
    
    private IEnumerator MistakeVisualizerCoroutine(float seconds)
    {
        mistakeVisualizer.SetActive(true);

        var visualizerImage = mistakeVisualizer.GetComponent<Image>();

        var blinkInterval = 0.1f;
        var blinkCount = Mathf.FloorToInt(seconds / (2 * blinkInterval));

        for (var i = 0; i < blinkCount; i++)
        {
            SetImageAlpha(visualizerImage, 0);
            yield return new WaitForSeconds(blinkInterval);

            SetImageAlpha(visualizerImage, 1);
            yield return new WaitForSeconds(blinkInterval);
        }
        SetImageAlpha(visualizerImage, 1);
        mistakeVisualizer.SetActive(false);
    }
    
    private static void SetImageAlpha(Graphic image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void StartSorting()
    {
        var gameManager = GameManager.Singleton;
        var game = gameManager.Game;
        
        CreateArray(game.SortingAlgorithm, game.ArraySize, game.SortType);
        
        gameManager.isGameRunning = true;
        gameManager.isGamePaused = false;
        gameManager.mistakeCount = 0;
        mistakeCountText.text = "Mistakes: 0";
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
        gameManager.isGameRunning = false;
        // TODO: gameManager.gameState.SaveHighscore(timerText.text);
        // TODO: send the highscore to the server -> maybe something like finishedSorting with 1 for sorted and 0 for canceled game + additional info like time, mistakes, etc.
        StopAllCoroutines();
        ArrayView?.DestroyArray();
        winPanel.SetActive(true);
        winText.text = $"You finished in {timer.GetTime()} with {gameManager.mistakeCount} mistakes!";
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
    
    public void DestroyGame()
    {
        var gameManager = GameManager.Singleton;
        winPanel.SetActive(false);
        mistakeCountText.text = "Mistakes: 0";
        gameManager.mistakeCount = 0;
        timer.StopTimer();
        StopAllCoroutines();
        ArrayView?.DestroyArray();
    }
}