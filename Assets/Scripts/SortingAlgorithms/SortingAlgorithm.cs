using System.Collections;
using System.Collections.Generic;
using Adlete;
using Configs;
using Enums;
using GameUI;
using UnityEngine;

namespace SortingAlgorithms
{
    public abstract class SortingAlgorithm
    {
        internal ArrayView ArrayView;
        public readonly List<(int index1, int index2, bool swap, int end)> Steps = new();
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
        
        // protected abstract EBarEffect GetEffect(int index, int currentStepIndex, int end);
        
        // protected abstract EBarEffect GetEffect(int index, int index1, int index2, int end);
        
        protected abstract void ApplyEffects((int index1, int index2, bool swap, int end) step);
        
        // TODO: implement a Tutorial for visualizing the sorting algorithm step by step and track the amount of how often the user has watched the tutorial
        public IEnumerator VisualizeSort()
        {
            var gameManager = GameManager.Singleton;
            // do the Bubble Sort algorithm step by step
            while (_currentStepIndex < Steps.Count)
            {
                // wait until the game is not paused
                yield return new WaitUntil(() => gameManager.isGamePaused == false);
                
                var step = GetNextStep();
                if (step.index1 == -1 || step.index2 == -1)
                {
                    break;
                }

                // apply the effect to the elements
                ApplyEffects(step);
                
                yield return new WaitForSeconds(_arraySettings.sortingSpeed);
                
                // swap the elements
                if (step.swap)
                {
                    ArrayView.SwapElements(step.index1, step.index2);
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
            var moduleConnection = ModuleConnection.Singleton;
            var gameSettings = gameManager.gameSettings;
            if (gameSettings.showDebugLogs)
            {
                Debug.Log("PlaySort");
            }
            var mistake = 0;
            while(_currentStepIndex < Steps.Count)
            {
                // wait until the game is not paused
                yield return new WaitUntil(() => gameManager.isGamePaused == false);
                
                var step = GetNextStep();
                if (step.index1 == -1 || step.index2 == -1)
                {
                    break;
                }
                
                // apply the effect to the elements, e.g. highlight the elements that are compared
                ApplyEffects(step);

                // Wait until the left or right arrow key or the according button is pressed
                yield return new WaitUntil(() =>
                    InputManager.Singleton.GetLeftInput() || InputManager.Singleton.GetRightInput());

                // Check which key was pressed
                if (InputManager.Singleton.GetLeftInput())
                {
                    if (step.swap)
                    {
                        ArrayView.SwapElements(step.index1, step.index2);
                        _currentStepIndex++;
                        // var observation = new Observation
                        // {
                        //     activityName = "activityAddition",
                        //     activityCorrectness = 1,
                        //     activityDifficulty = 0.5f,
                        //     timestamp = DateTime.Now,
                        //     additionalInfos = "{\"data\":10}"
                        // };
                        // moduleConnection.SubmitActivityResult(observation);
                    }
                    else
                    {
                        mistake++;
                        _levelManager.IncreaseMistakeCount();
                        if(gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        _levelManager.ShowMistakeVisualizer(_arraySettings.errorCooldown);
                        yield return new WaitForSeconds(_arraySettings.errorCooldown);
                    }
                }
                else if (InputManager.Singleton.GetRightInput())
                {
                    if (step.swap)
                    {
                        mistake++;
                        _levelManager.IncreaseMistakeCount();
                        if (gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        _levelManager.ShowMistakeVisualizer(_arraySettings.errorCooldown);
                        yield return new WaitForSeconds(_arraySettings.errorCooldown);
                    }
                    else
                    {
                        _currentStepIndex++;
                        // var observation = new Observation
                        // {
                        //     activityName = "activitySubtraction",
                        //     activityCorrectness = 1,
                        //     activityDifficulty = 0.5f,
                        //     timestamp = DateTime.Now,
                        //     additionalInfos = "{\"data\":10}"
                        // };
                        // moduleConnection.SubmitActivityResult(observation);
                    }
                }
            }
            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
            Debug.Log($"Sorting finished with {mistake} mistakes.");
            _levelManager.FinishSorting();
        }

        private (int index1, int index2, bool swap, int end) GetNextStep()
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
                _ when GetType() == typeof(SelectionSort) => ESortingAlgorithm.SelectionSort,
                _ when GetType() == typeof(InsertionSort) => ESortingAlgorithm.InsertionSort,
                _ => ESortingAlgorithm.BubbleSort
            };
        }
    }
}