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

    public void StartGame()
    {
        var gameManager = GameManager.Singleton;
        var sortingAlgorithm = algorithmSelector.GetSortingAlgorithm();
        var sortType = sortTypeToggle.GetSortType();
        var arraySize = (int)arraySizeSlider.GetValue();
        if (gameManager.gameSettings.devMode)
        {
            Debug.Log($"StartGame: {sortingAlgorithm}, {sortType}, {arraySize}");
        }
        
        gameManager.StartLevel(sortingAlgorithm, sortType, arraySize);
    }
}