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
        
        private LevelManager _levelManager;
        private ArraySettings _arraySettings;

        public void Init(LevelManager levelManager, ArrayView arrayView, ArraySettings arraySettings)
        {
            _levelManager = levelManager;
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
                _currentStepIndex++;
            }
            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
        }

        public IEnumerator PlaySort()
        {
            var gameManager = GameManager.Singleton;
            var gameSettings = gameManager.gameSettings;
            if (gameSettings.showDebugLogs)
            {
                Debug.Log("PlaySort");
            }
            var mistake = 0;
            while(_currentStepIndex < Steps.Count)
            {
                var (index1, index2, swap, end) = GetNextStep();
                if (index1 == -1 || index2 == -1)
                {
                    break;
                }
                for (var i = 0; i < ArrayView.ArraySize; i++)
                {
                    var effect = GetEffect(i, index1, end);
                    ArrayView.ApplyBarEffect(i, effect);
                }
                // Wait until the left or right arrow key is pressed
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow));
        
                // Check which key was pressed
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if(swap)
                    {
                        ArrayView.SwapElements(index1, index2);
                        _currentStepIndex++;
                    }
                    else
                    {
                        mistake++;
                        _levelManager.IncreaseMistakeCount();
                        if(gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        yield return new WaitForSeconds(_arraySettings.errorCooldown);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (swap)
                    {
                        mistake++;
                        _levelManager.IncreaseMistakeCount();
                        if (gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        yield return new WaitForSeconds(_arraySettings.errorCooldown);
                    }
                    else
                    {
                        _currentStepIndex++;
                    }
                }
            }
            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
            Debug.Log($"Sorting finished with {mistake} mistakes.");
        }

        private (int, int, bool, int) GetNextStep()
        {
            if (_currentStepIndex >= Steps.Count)
            {
                return (-1, -1, false, ArrayView.ArraySize);
            }
            var step = Steps[_currentStepIndex];
            return step;
        }
        
        public ESortingAlgorithm GetSortingAlgorithm()
        {
            return GetType() switch
            {
                _ when GetType() == typeof(BubbleSort) => ESortingAlgorithm.BubbleSort,
                _ when GetType() == typeof(BubbleSort) => ESortingAlgorithm.SelectionSort, // TODO: implement SelectionSort
                _ when GetType() == typeof(BubbleSort) => ESortingAlgorithm.InsertionSort, // TODO: implement InsertionSort
                _ => ESortingAlgorithm.BubbleSort
            };
        }
    }
}