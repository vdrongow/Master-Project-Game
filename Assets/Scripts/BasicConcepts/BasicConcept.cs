using System;
using Manager;
using Random = UnityEngine.Random;

namespace BasicConcepts
{
    public abstract class BasicConcept
    {
        internal LevelBasicsManager LevelBasicsManager;
        
        public void Init(LevelBasicsManager levelBasicsManager)
        {
            LevelBasicsManager = levelBasicsManager;
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

        public abstract void OnArrayElementClicked(ArrayElement arrayElement);
        
        public abstract string GetTaskAsString();
    }
}