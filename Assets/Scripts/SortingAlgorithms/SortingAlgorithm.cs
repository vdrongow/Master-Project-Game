using System.Collections;
using System.Collections.Generic;
using Configs;
using Enums;
using UnityEngine;

namespace SortingAlgorithms
{
    public abstract class SortingAlgorithm
    {
        internal ArrayView ArrayView;
        internal readonly List<(int index1, int index2, bool swap, int end)> Steps = new();
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
        
        protected abstract EBarEffect GetEffect(int index, int currentStepIndex, int end);
        
        // TODO: implement a Tutorial for visualizing the sorting algorithm step by step and track the amount of how often the user has watched the tutorial
        public IEnumerator VisualizeSort()
        {
            // do the Bubble Sort algorithm step by step
            while (_currentStepIndex < Steps.Count)
            {
                var (index1, index2, swap, end) = GetNextStep();
                if (index1 == -1 || index2 == -1)
                {
                    break;
                }

                // apply the effect to the elements
                for (var i = 0; i < ArrayView.ArraySize; i++)
                {
                    var effect = GetEffect(i, index1, end);
                    ArrayView.ApplyBarEffect(i, effect);
                }
                
                yield return new WaitForSeconds(_arraySettings.sortingSpeed);
                
                // swap the elements
                if (swap)
                {
                    ArrayView.SwapElements(index1, index2);
                    yield return new WaitForSeconds(_arraySettings.sortingSpeed);
                }
            }
            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
        }

        private (int, int, bool, int) GetNextStep()
        {
            if (_currentStepIndex >= Steps.Count)
            {
                return (-1, -1, false, ArrayView.ArraySize);
            }
            var step = Steps[_currentStepIndex];
            _currentStepIndex++;
            return step;
        }
        
    }
}