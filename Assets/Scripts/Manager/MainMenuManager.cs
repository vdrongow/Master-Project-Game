using Adlete;
using GameUI;
using UnityEngine;

namespace Manager
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private AlgorithmOptionSelector algorithmSelector;
        [SerializeField]
        private SortTypeToggle sortTypeToggle;
        [SerializeField]
        private ArraySizeSlider arraySizeSlider;
    
        private void Start()
        {
            var moduleConnection = ModuleConnection.Singleton;
            Debug.Log($"Logged in as {moduleConnection.GetLoggedInUser()}");
        }

        public void StartSortingGame()
        {
            var gameManager = GameManager.Singleton;
            var sortingAlgorithm = algorithmSelector.GetSortingAlgorithm();
            var sortType = sortTypeToggle.GetSortType();
            var arraySize = (int)arraySizeSlider.GetValue();
            if (gameManager.gameSettings.devMode)
            {
                Debug.Log($"StartGame: {sortingAlgorithm}, {sortType}, {arraySize}");
            }
        
            gameManager.StartSortingLevel(sortingAlgorithm, sortType, arraySize);
        }
    }
}