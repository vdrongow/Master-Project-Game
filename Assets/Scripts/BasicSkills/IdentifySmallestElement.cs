using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace BasicSkills
{
    public sealed class IdentifySmallestElement : BasicSkill
    {
        private const string TaskTitle = "Pick the Smallest Element";
        private List<ArrayElement> _arrayElements = new();
        private ArrayElement _smallestElement;
        
        public override string GetTaskTitle() => TaskTitle;

        protected override void InitTask()
        {
            var gameSettings = GameManager.Singleton.gameSettings;
            DestroyTask();
            var size = gameSettings.defaultBasicLevelElements;
            var array = CreateArray(size);
            // visualise the array using bars with different heights according to the values
            _arrayElements = InitArrayElements(array);
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
            var task = $"smallest Element: {_smallestElement.Value} \n";
            task += string.Join(", \n",
                _arrayElements.Select((arrayEl, index) =>
                    $"[{index.ToString()}] {arrayEl.Value}"));
            return task;
        }
    }
}