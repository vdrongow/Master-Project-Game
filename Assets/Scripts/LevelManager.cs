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

    private void StartSorting()
    {
        var gameManager = GameManager.Singleton;
        gameManager.isGamePaused = false;
        gameManager.isGameStarted = true;
        
        CreateArray(gameManager.sortingAlgorithm, gameManager.arraySize, gameManager.sortType);
        StartCoroutine(SortingAlgorithm.PlaySort());
    }

    public void CreateArray(ESortingAlgorithm sortingAlgorithm, int arraySize, ESortType sortType)
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