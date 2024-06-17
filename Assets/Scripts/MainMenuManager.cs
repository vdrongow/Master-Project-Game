using GameUI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private AlgorithmOptionSelector algorithmSelector;
    [SerializeField]
    private SortTypeToggle sortTypeToggle;
    [SerializeField]
    private ArraySizeSlider arraySizeSlider;

    private void Awake()
    {
        
    }

    public void StartGame()
    {
        var sortingAlgorithm = algorithmSelector.GetSortingAlgorithm();
        var sortType = sortTypeToggle.GetSortType();
        var arraySize = (int)arraySizeSlider.GetValue();
        Debug.Log($"StartGame: {sortingAlgorithm}, {sortType}, {arraySize}");
        var gameManager = GameManager.Singleton;
        gameManager.LoadNextScene();
    }

    public void Click()
    {
        var gameManager = GameManager.Singleton;
        var gameState = gameManager.gameState;
        
        gameState.Count++;
    }
}