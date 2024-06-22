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
        private readonly List<(int index1, int index2, bool swap, int end)> Steps = new();
        private int _currentStepIndex;
        
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // break down the Selection Sort algorithm into steps
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();

            for (var i = 0; i < array.Length - 1; i++)
            {
                var minIndex = i;
                for (var j = i + 1; j < array.Length; j++)
                {
                    Steps.Add((j, minIndex, swap: false, i));
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    Steps.Add((i, minIndex, swap: true, array.Length - 1));
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
                if (step.index1 == -1 || step.index2 == -1)
                {
                    break;
                }

                // apply the effect to the elements
                ApplyEffects(step);

                yield return new WaitForSeconds(ArraySettings.sortingSpeed);

                // swap the elements
                if (step.swap)
                {
                    ArrayView.SwapElements(step.index1, step.index2);
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
                        LevelManager.IncreaseMistakeCount();
                        if(gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        LevelManager.ShowMistakeVisualizer(ArraySettings.errorCooldown);
                        yield return new WaitForSeconds(ArraySettings.errorCooldown);
                    }
                }
                else if (InputManager.Singleton.GetRightInput())
                {
                    if (step.swap)
                    {
                        mistake++;
                        LevelManager.IncreaseMistakeCount();
                        if (gameSettings.showDebugLogs)
                        {
                            Debug.Log("Mistake!");
                        }
                        LevelManager.ShowMistakeVisualizer(ArraySettings.errorCooldown);
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
            LevelManager.FinishSorting();
        }

        private void ApplyEffects((int index1, int index2, bool swap, int end) step)
        {
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                var effect = GetEffect(i, step.index1, step.index2, step.end);
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
            if (i <= end)
            {
                return EBarEffect.Sorted;
            }
            return EBarEffect.None;
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
        
        public override string GetStepsAsString()
        {
            return string.Join(", \n",
                Steps.Select((step, index) =>
                    $"[{index.ToString()}] (index1: {step.index1}, index2: {step.index2}, swap: {step.swap}, end: {step.end})"));
        }
    }
}