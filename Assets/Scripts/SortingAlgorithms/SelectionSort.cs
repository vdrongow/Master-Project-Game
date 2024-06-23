using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adlete;
using Enums;
using GameUI;
using UnityEngine;

namespace SortingAlgorithms
{
    public sealed class SelectionSort : SortingAlgorithm
    {
        private readonly List<(int index, int minIndex, bool foundNewMin, bool swap, int end)> Steps = new();
        private int _currentStepIndex;
        
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // Break down the Selection Sort algorithm into steps
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();

            for (var i = 0; i < array.Length - 1; i++)
            {
                var minIndex = i;
                for (var j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        Steps.Add((j, minIndex, foundNewMin: true, swap: false, i));
                        minIndex = j;
                    }
                    else
                    {
                        Steps.Add((j, minIndex, foundNewMin: false, swap: false, i));
                    }
                }

                if (minIndex != i)
                {
                    Steps.Add((i, minIndex, foundNewMin: false,  swap: true, i));
                    (array[i], array[minIndex]) = (array[minIndex], array[i]);
                }
            }
        }
        
        public override IEnumerator VisualizeSort()
        {
            var gameManager = GameManager.Singleton;
            // do the Bubble Sort algorithm step by step
            while (_currentStepIndex < Steps.Count)
            {
                // wait until the game is not paused
                yield return new WaitUntil(() => gameManager.isGamePaused == false);

                var step = GetNextStep();
                if (step.index == -1 || step.minIndex == -1)
                {
                    break;
                }

                // apply the effect to the elements
                ApplyEffects(step);

                yield return new WaitForSeconds(ArraySettings.sortingSpeed);

                // swap the elements
                if (step.swap)
                {
                    ArrayView.SwapElements(step.index, step.minIndex);
                    yield return new WaitForSeconds(ArraySettings.sortingSpeed);
                }

                _currentStepIndex++;
            }

            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
        }

        public override IEnumerator PlaySort()
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
                if (step.index == -1 || step.minIndex == -1)
                {
                    break;
                }
                
                // apply the effect to the elements, e.g. highlight the elements that are compared
                ApplyEffects(step);
                
                if (step.swap)
                {
                    ArrayView.SwapElements(step.index, step.minIndex);
                    _currentStepIndex++;
                    continue;
                }

                // Wait until the left or right arrow key or the according button is pressed
                yield return new WaitUntil(() =>
                    InputManager.Singleton.GetLeftInput() || InputManager.Singleton.GetRightInput());

                // Check which key was pressed
                if (InputManager.Singleton.GetLeftInput())
                {
                    if (step.foundNewMin)
                    {
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
                        LevelSortingManager.IncreaseMistakeCount();
                        if(gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        LevelSortingManager.ShowMistakeVisualizer(ArraySettings.errorCooldown);
                        yield return new WaitForSeconds(ArraySettings.errorCooldown);
                    }
                }
                else if (InputManager.Singleton.GetRightInput())
                {
                    if (step.foundNewMin)
                    {
                        mistake++;
                        LevelSortingManager.IncreaseMistakeCount();
                        if (gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        LevelSortingManager.ShowMistakeVisualizer(ArraySettings.errorCooldown);
                        yield return new WaitForSeconds(ArraySettings.errorCooldown);
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
            LevelSortingManager.FinishSorting();
        }

        private void ApplyEffects((int index, int minIndex, bool foundNewMin, bool swap, int end) step)
        {
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                var effect = GetEffect(i, step.index, step.minIndex, step.end);
                ArrayView.ApplyBarEffect(i, effect);
            }
        }

        private EBarEffect GetEffect(int i, int currentIndex, int minIndex, int end)
        {
            if (i == currentIndex)
            {
                return EBarEffect.Highlight;
            }
            if (i == minIndex)
            {
                return EBarEffect.HighlightCurrent;
            }
            if (i < end)
            {
                return EBarEffect.Sorted;
            }
            return EBarEffect.None;
        }
        
        private (int index, int minIndex, bool foundNewMin, bool swap, int end) GetNextStep()
        {
            if (_currentStepIndex >= Steps.Count)
            {
                return (-1, -1, false, false, ArrayView.ArraySize);
            }
            var step = Steps[_currentStepIndex];
            return step;
        }
        
        public override string GetStepsAsString()
        {
            return string.Join(", \n",
                Steps.Select((step, index) =>
                    $"[{index.ToString()}] (minIndex: {step.minIndex}, index: {step.index}, foundNewMin: {step.foundNewMin}, swap: {step.swap}, end: {step.end})"));
        }
        
        public override string GetCurrentStepAsString() => _currentStepIndex >= Steps.Count
            ? "Sorting finished"
            : $"currentStepIndex: [{_currentStepIndex}]";
    }
}