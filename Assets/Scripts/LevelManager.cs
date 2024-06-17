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
    
    public ArrayView ArrayView;
    public SortingAlgorithm SortingAlgorithm;

    private void Awake()
    {
        // delete all childs of the array parent
        foreach (Transform child in arrayParent.transform)
        {
            Destroy(child.gameObject);
        }

        var gameManager = GameManager.Singleton;
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
        gameManager.LoadNextScene();
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
        
        CreateArray(gameManager.sortingAlgorithm, gameManager.arraySize, gameManager.sortType);
        
        gameManager.isGameRunning = true;
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
        
        SortingAlgorithm.Init(ArrayView, GameManager.Singleton.arraySettings);
    }
}