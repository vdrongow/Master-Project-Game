using System.Collections;
using Configs;
using Enums;

namespace SortingAlgorithms
{
    public abstract class SortingAlgorithm
    {
        internal ArrayView ArrayView;
        internal LevelManager LevelManager;
        internal ArraySettings ArraySettings;

        public void Init(LevelManager levelManager, ArrayView arrayView, ArraySettings arraySettings)
        {
            LevelManager = levelManager;
            ArrayView = arrayView;
            ArraySettings = arraySettings;
            PrepareSteps();
        }

        protected abstract void PrepareSteps();
        
        public abstract IEnumerator VisualizeSort();
        
        public abstract IEnumerator PlaySort();

        public abstract string GetStepsAsString();

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