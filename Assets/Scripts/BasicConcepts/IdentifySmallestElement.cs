using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace BasicConcepts
{
    public sealed class IdentifySmallestElement : BasicConcept
    {
        private List<ArrayElement> _arrayElements = new();
        private ArrayElement _smallestElement;

        protected override void InitGame()
        {
            var gameSettings = GameManager.Singleton.gameSettings;
            var arraySettings = GameManager.Singleton.arraySettings;
            var size = gameSettings.defaultArraySize;
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
            _smallestElement = _arrayElements.Min();
        }

        public override void OnArrayElementClicked(ArrayElement arrayElement)
        {
            if (arrayElement == _smallestElement)
            {
                LevelBasicsManager.IncreaseScoreCount();
            }
            else
            {
                LevelBasicsManager.IncreaseMistakeCount();
            }
        }
    }
}