using System.Collections;
using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace SortingAlgorithms
{
    public abstract class SortingAlgorithm
    {
        internal ArrayView ArrayView;
        internal readonly List<(int index1, int index2, bool swap)> Steps = new();
        private int _currentStepIndex;
        
        private ArraySettings _arraySettings;

        public void Init(ArrayView arrayView, ArraySettings arraySettings)
        {
            ArrayView = arrayView;
            _arraySettings = arraySettings;
            PrepareSteps();
            _currentStepIndex = 0;
        }

        protected abstract void PrepareSteps();
        
        // TODO: implement a Tutorial for visualizing the sorting algorithm step by step and track the amount of how often the user has watched the tutorial
        public IEnumerator VisualizeSort()
        {
            // do the Bubble Sort algorithm step by step
            while (!ArrayView.IsSorted())
            {
                var (index1, index2, swap) = GetNextStep();
                if (index1 == -1 || index2 == -1)
                {
                    break;
                }
                // highlight the elements that are being compared
                ArrayView.ClearHighlights();
                ArrayView.HighlightElement(index1);
                ArrayView.HighlightElement(index2);
                yield return new WaitForSeconds(_arraySettings.sortingSpeed);
                // swap the elements
                if (swap)
                {
                    ArrayView.SwapElements(index1, index2);
                }
            }
            
            // if the array is type sorted, still highlight the steps
            if(ArrayView.SortType == Enums.ESortType.Sorted)
            {
                for (var i = 0; i < ArrayView.ArraySize - 1; i++)
                {
                    ArrayView.ClearHighlights();
                    ArrayView.HighlightElement(i);
                    ArrayView.HighlightElement(i + 1);
                    yield return new WaitForSeconds(_arraySettings.sortingSpeed);
                }
            }
            
            // clear highlights
            ArrayView.ClearHighlights();
        }
        
        private (int, int, bool) GetNextStep()
        {
            if (_currentStepIndex >= Steps.Count)
            {
                return (-1, -1, false);
            }
            var step = Steps[_currentStepIndex];
            _currentStepIndex++;
            return step;
        }
        
    }
}