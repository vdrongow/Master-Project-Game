using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace BasicConcepts
{
    public sealed class IdentifySmallestElement : BasicConcept
    {
        private string _taskTitle = "Pick the Smallest Element";
        private List<ArrayElement> _arrayElements = new();
        private ArrayElement _smallestElement;
        
        public override string GetTaskTitle() => _taskTitle;

        protected override void InitTask()
        {
            var gameSettings = GameManager.Singleton.gameSettings;
            var arraySettings = GameManager.Singleton.arraySettings;
            DestroyArrayElements();
            var size = gameSettings.defaultBasicLevelElements;
            var array = CreateArray(size);
            // visualise the array using bars with different heights according to the values
            for (var i = 0; i < size; i++)
            {
                var go = Object.Instantiate(arraySettings.barPrefab, LevelBasicsManager.contentParent.transform);
                var arrayElement = go.GetComponent<ArrayElement>();
                arrayElement.Init(array[i], () => OnArrayElementClicked(arrayElement));
                _arrayElements.Add(arrayElement);
            }
            // find the smallest element in the array
            _smallestElement = _arrayElements.OrderBy(x => x.Value).First();
        }

        public override void OnArrayElementClicked(ArrayElement arrayElement)
        {
            if (arrayElement == _smallestElement)
            {
                LevelBasicsManager.IncreaseScoreCount();
                InitTask();
            }
            else
            {
                LevelBasicsManager.IncreaseMistakeCount();
            }
        }
        
        private void DestroyArrayElements()
        {
            foreach (var arrayElement in _arrayElements)
            {
                Object.Destroy(arrayElement.gameObject);
            }
            _arrayElements.Clear();
        }
    }
}