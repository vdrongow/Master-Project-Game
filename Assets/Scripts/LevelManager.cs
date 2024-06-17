using Enums;
using SortingAlgorithms;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject arrayParent;
    
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
        gameManager.isGamePaused = false;
        
        CreateArray(gameManager.sortingAlgorithm, gameManager.arraySize, gameManager.sortType);
        StartCoroutine(SortingAlgorithm.PlaySort());
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