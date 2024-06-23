using System.Collections;
using Configs;
using Enums;
using Manager;

namespace SortingAlgorithms
{
    public abstract class SortingAlgorithm
    {
        internal ArrayView ArrayView;
        internal LevelSortingManager LevelSortingManager;
        internal ArraySettings ArraySettings;

        public void Init(LevelSortingManager levelSortingManager, ArrayView arrayView, ArraySettings arraySettings)
        {
            LevelSortingManager = levelSortingManager;
            ArrayView = arrayView;
            ArraySettings = arraySettings;
            PrepareSteps();
        }

        protected abstract void PrepareSteps();
        
        public abstract IEnumerator VisualizeSort();
        
        public abstract IEnumerator PlaySort();

        public abstract string GetStepsAsString();
        
        public abstract string GetCurrentStepAsString();

        public ESortingAlgorithm GetSortingAlgorithm()
        {
            return GetType() switch
            {
                _ when GetType() == typeof(BubbleSort) => ESortingAlgorithm.BubbleSort,
                _ when GetType() == typeof(SelectionSort) => ESortingAlgorithm.SelectionSort,
                _ when GetType() == typeof(InsertionSort) => ESortingAlgorithm.InsertionSort,
                _ => ESortingAlgorithm.BubbleSort
            };
        }
    }
}