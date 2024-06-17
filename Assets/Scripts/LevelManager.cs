using Enums;
using SortingAlgorithms;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject arrayParent;
    [SerializeField]
    private MySlider arraySizeSlider;
    
    public ArrayView ArrayView;

    private void Awake()
    {
        var gameManager = GameManager.Singleton;

        arraySizeSlider.Init();
        arraySizeSlider.SetMinValue(gameManager.arraySettings.minArraySize);
        arraySizeSlider.SetMaxValue(gameManager.arraySettings.maxArraySize);
        var handleValue = (gameManager.arraySettings.minArraySize + gameManager.arraySettings.maxArraySize) / 2;
        arraySizeSlider.SetValue(handleValue);
        // delete all childs of the array parent
        foreach (Transform child in arrayParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }

        if(ArrayView != null)
        {
            // press V for visualizing the sorting algorithm
            if (Input.GetKeyDown(KeyCode.V))
            {
                // start sorting
                var bubbleSort = new BubbleSort();
                bubbleSort.Init(ArrayView, GameManager.Singleton.arraySettings);
                StartCoroutine(bubbleSort.VisualizeSort());
            }
            // press P for playing the sorting algorithm
            if (Input.GetKeyDown(KeyCode.P))
            {
                // start sorting
                var bubbleSort = new BubbleSort();
                bubbleSort.Init(ArrayView, GameManager.Singleton.arraySettings);
                StartCoroutine(bubbleSort.PlaySort());
            }
        }
    }

    public void BackToMainMenu()
    {
        var gameManager = GameManager.Singleton;
        gameManager.LoadNextScene();
    }

    public void CreateArray(int sortType)
    {
        var gameManager = GameManager.Singleton;
        var arraySettings = gameManager.arraySettings;
        var arraySize = (int)arraySizeSlider.GetValue();

        StopAllCoroutines();
        
        ArrayView?.DestroyArray();
        ArrayView = new ArrayView(arrayParent, arraySize, arraySettings, (ESortType)sortType);
    }
    
    public void Click()
    {
        var gameManager = GameManager.Singleton;
        var gameState = gameManager.gameState;
        
        gameState.Count++;
    }
}