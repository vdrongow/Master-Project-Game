using Adlete;
using Enums;
using GameUI;
using UnityEngine;

namespace Manager
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Sorting Game References")]
        [SerializeField]
        private OptionSelector algorithmSelector;
        [SerializeField]
        private SortTypeToggle sortTypeToggle;
        [SerializeField]
        private ArraySizeSlider arraySizeSlider;
        
        [Header("Basic Game References")]
        [SerializeField]
        private OptionSelector basicSkillSelector;
    
        private void Start()
        {
            var moduleConnection = ModuleConnection.Singleton;
            Debug.Log($"Logged in as {moduleConnection.GetLoggedInUser()}");
        }

        public void StartSortingGame()
        {
            var gameManager = GameManager.Singleton;
            var sortingAlgorithm = (ESortingAlgorithm) algorithmSelector.GetCurrentIndex();
            var sortType = sortTypeToggle.GetSortType();
            var arraySize = (int)arraySizeSlider.GetValue();
            if (gameManager.gameSettings.devMode)
            {
                Debug.Log($"StartGame: {sortingAlgorithm}, {sortType}, {arraySize}");
            }
        
            gameManager.StartSortingLevel(sortingAlgorithm, sortType, arraySize);
        }
        
        public void StartBasicGame()
        {
            var gameManager = GameManager.Singleton;
            var basicSkill = (EBasicSkill) basicSkillSelector.GetCurrentIndex();
            if (gameManager.gameSettings.devMode)
            {
                Debug.Log($"StartGame: {basicSkill}");
            }
        
            gameManager.StartBasicLevel(basicSkill);
        }
        
        public void CloseGame()
        {
            var gameManager = GameManager.Singleton;
            gameManager.CloseGame();
        }
    }
}