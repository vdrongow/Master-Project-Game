using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Manager;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace BasicSkills
{
    public sealed class IdentifyLargestElement : BasicSkill
    {
        private const string TaskTitle = "Pick the Largest Element";
        private List<ArrayElement> _arrayElements = new();
        private ArrayElement _largestElement;
        
        public override string GetTaskTitle() => TaskTitle;

        protected override void InitTask()
        {
            DestroyTask();
            var size = GameSettings.defaultBasicLevelElements;
            var array = CreateArray(size);
            // visualise the array using bars with different heights according to the values
            _arrayElements = InitArrayElements(array);
            // find the largest element in the array
            _largestElement = _arrayElements.OrderByDescending(x => x.Value).First();
        }

        private int[] CreateArray(int size)
        {
            var randomMaxOffset = Random.Range(1, 10);
            ArraySettings.maxValue = ArraySettings.minValue + size - 1 + randomMaxOffset;
            var array = new int[size];
            for (var i = 0; i < size; i++)
            {
                int value;
                do
                {
                    value = Random.Range(ArraySettings.minValue, ArraySettings.maxValue + 1);
                } while (Array.IndexOf(array, value) != -1);
                array[i] = value;
            }
            return array;
        }

        private List<ArrayElement> InitArrayElements(int[] array)
        {
            var size = array.Length;
            var arrayElements = new List<ArrayElement>();
            for (var i = 0; i < size; i++)
            {
                var go = Object.Instantiate(ArraySettings.barPrefab, LevelBasicsManager.arrayParent.transform);
                var arrayElement = go.GetComponent<ArrayElement>();
                arrayElement.Init(array[i], () => OnArrayElementClicked(arrayElement));
                arrayElements.Add(arrayElement);
            }

            return arrayElements;
        }

        private void OnArrayElementClicked(ArrayElement arrayElement)
        {
            var gameManager = GameManager.Singleton;
            if (arrayElement == _largestElement)
            {
                LevelBasicsManager.IncreaseScoreCount();
                gameManager.SubmitActivityResult(EActivityType.IdentifyLargestElement, 1);
                InitTask();
            }
            else
            {
                LevelBasicsManager.IncreaseMistakeCount();
                gameManager.SubmitActivityResult(EActivityType.IdentifyLargestElement, 0);
            }
        }
        
        public override string GetTaskAsString()
        {
            var task = $"largest Element: {_largestElement.Value} \n";
            task += string.Join(", \n",
                _arrayElements.Select((arrayEl, index) =>
                    $"[{index.ToString()}] {arrayEl.Value}"));
            return task;
        }
        
        public override void DestroyTask()
        {
            foreach (var arrayElement in _arrayElements)
            {
                Object.Destroy(arrayElement.gameObject);
            }
            _arrayElements.Clear();
        }
    }
}