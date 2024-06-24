using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

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
            var gameSettings = GameManager.Singleton.gameSettings;
            DestroyTask();
            var size = gameSettings.defaultBasicLevelElements;
            var array = CreateArray(size);
            // visualise the array using bars with different heights according to the values
            _arrayElements = InitArrayElements(array);
            // find the largest element in the array
            _largestElement = _arrayElements.OrderByDescending(x => x.Value).First();
        }

        public override void OnArrayElementClicked(ArrayElement arrayElement)
        {
            if (arrayElement == _largestElement)
            {
                LevelBasicsManager.IncreaseScoreCount();
                InitTask();
            }
            else
            {
                LevelBasicsManager.IncreaseMistakeCount();
            }
        }
        
        public override void DestroyTask()
        {
            foreach (var arrayElement in _arrayElements)
            {
                Object.Destroy(arrayElement.gameObject);
            }
            _arrayElements.Clear();
        }

        public override string GetTaskAsString()
        {
            var task = $"largest Element: {_largestElement.Value} \n";
            task += string.Join(", \n",
                _arrayElements.Select((arrayEl, index) =>
                    $"[{index.ToString()}] {arrayEl.Value}"));
            return task;
        }
    }
}