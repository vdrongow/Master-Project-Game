using System;
using Enums;
using SortingAlgorithms;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject arrayParent;
    [SerializeField]
    private MySlider arraySizeSlider;
    
    public ArrayView ArrayView;

    private int _randomIndex = -1;
    private int _nextIndex = -1;
    
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HighlightRandomElements();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(_randomIndex == -1 || _nextIndex == -1)
            {
                return;
            }
            ArrayView.SwapElements(_randomIndex, _nextIndex);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // start sorting
            var bubbleSort = new BubbleSort();
            bubbleSort.Init(ArrayView, GameManager.Singleton.arraySettings);
            StartCoroutine(bubbleSort.VisualizeSort());
        }
    }

    private void HighlightRandomElements()
    {
        if(ArrayView == null)
        {
            return;
        }
        
        ArrayView.ClearHighlights();
        _randomIndex = Random.Range(0, ArrayView.ArraySize - 1);
        _nextIndex = _randomIndex + 1;
        
        ArrayView.HighlightElement(_randomIndex);
        ArrayView.HighlightElement(_nextIndex);
        
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