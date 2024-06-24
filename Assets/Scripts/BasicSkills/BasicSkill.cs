using System;
using System.Collections.Generic;
using Configs;
using Manager;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace BasicSkills
{
    public abstract class BasicSkill
    {
        internal LevelBasicsManager LevelBasicsManager;
        internal GameSettings GameSettings;
        internal ArraySettings ArraySettings;
        
        public void Init(LevelBasicsManager levelBasicsManager)
        {
            LevelBasicsManager = levelBasicsManager;
            var gameManager = GameManager.Singleton;
            GameSettings = gameManager.gameSettings;
            ArraySettings = gameManager.arraySettings;
            InitTask();
        }

        public abstract string GetTaskTitle();
        
        protected abstract void InitTask();
        
        protected int[] CreateArray(int size)
        {
            var arraySettings = GameManager.Singleton.arraySettings;
            arraySettings.maxValue = arraySettings.minValue + size - 1;
            var array = new int[size];
            for (var i = 0; i < size; i++)
            {
                int value;
                do
                {
                    value = Random.Range(arraySettings.minValue, arraySettings.maxValue + 1);
                } while (Array.IndexOf(array, value) != -1);
                array[i] = value;
            }
            return array;
        }

        protected List<ArrayElement> InitArrayElements(int[] array)
        {
            var size = array.Length;
            var arrayElements = new List<ArrayElement>();
            for (var i = 0; i < size; i++)
            {
                var go = UnityEngine.Object.Instantiate(ArraySettings.barPrefab, LevelBasicsManager.contentParent.transform);
                var arrayElement = go.GetComponent<ArrayElement>();
                arrayElement.Init(array[i], () => OnArrayElementClicked(arrayElement));
                arrayElements.Add(arrayElement);
            }

            return arrayElements;
        }

        public abstract void OnArrayElementClicked(ArrayElement arrayElement);
        
        public abstract string GetTaskAsString();
    }
}