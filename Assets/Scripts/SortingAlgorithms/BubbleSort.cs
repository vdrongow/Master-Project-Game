﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adlete;
using Enums;
using GameUI;
using Manager;
using UnityEngine;

namespace SortingAlgorithms
{
    public sealed class BubbleSort : SortingAlgorithm
    {
        private readonly List<(int index1, int index2, bool swap, int end)> Steps = new();
        private int _currentStepIndex;
        
        protected override void PrepareSteps()
        {
            Steps.Clear();
            // break down the algorithm into steps 
            var array = ArrayView.ArrayElements.Select(x => x.Value).ToArray();
            var isSorted = false;
            var end = array.Length;
            while (!isSorted)
            {
                isSorted = true;
                for (var i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        isSorted = false;
                        Steps.Add((i, i + 1, swap: true, end));
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    } 
                    else
                    {
                        // add a step without swapping to highlight the elements that are compared, but only if they are not already sorted
                        if(i + 1 < end)
                        {
                            Steps.Add((i, i + 1, swap: false, end));
                        }
                    }
                }
                end--;
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
                        gameManager.SubmitActivityResult(EActivityType.BubbleSortSwapElements, 1);
                    }
                    else
                    {
                        mistake++;
                        gameManager.SubmitActivityResult(EActivityType.BubbleSortStepOver, 0);
                        LevelSortingManager.IncreaseMistakeCount();
                        yield return new WaitForSeconds(gameSettings.errorCooldown);
                    }
                }
                else if (InputManager.Singleton.GetRightInput())
                {
                    if (step.swap)
                    {
                        mistake++;
                        gameManager.SubmitActivityResult(EActivityType.BubbleSortSwapElements, 0);
                        LevelSortingManager.IncreaseMistakeCount();
                        yield return new WaitForSeconds(gameSettings.errorCooldown);
                    }
                    else
                    {
                        _currentStepIndex++;
                        gameManager.SubmitActivityResult(EActivityType.BubbleSortStepOver, 1);
                    }
                }
            }
            // apply the sorted effect to all elements
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                ArrayView.ApplyBarEffect(i, EBarEffect.Sorted);
            }
            Debug.Log($"Sorting finished with {mistake} mistakes.");
            LevelSortingManager.EndGame();
        }

        private void ApplyEffects((int index1, int index2, bool swap, int end) step)
        {
            for (var i = 0; i < ArrayView.ArraySize; i++)
            {
                var effect = GetEffect(i, step.index1, step.index2, step.end);
                ArrayView.ApplyBarEffect(i, effect);
            }
        }
        
        private EBarEffect GetEffect(int i, int index1, int index2, int end)
        {
            if (i >= end)
            {
                return EBarEffect.Sorted;
            }
            if (i == index1 || i == index2)
            {
                return EBarEffect.Highlight;
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

        public override string GetCurrentStepAsString() => _currentStepIndex >= Steps.Count
            ? "Sorting finished"
            : $"currentStepIndex: [{_currentStepIndex}]";
    }
}