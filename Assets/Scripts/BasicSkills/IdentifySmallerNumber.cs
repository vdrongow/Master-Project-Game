using UnityEngine;

namespace BasicSkills
{
    public sealed class IdentifySmallerNumber : BasicSkill
    {
        private const string TaskTitle = "Pick the Smaller Number";
        private int _number1;
        private int _number2;
        private PickNumber _pickNumber;
        
        public override string GetTaskTitle() => TaskTitle;

        protected override void InitTask()
        {
            DestroyTask();
            
            // find two different random numbers
            var min = GameSettings.defaultMinValue;
            var max = GameSettings.defaultMaxValue;
            _number1 = Random.Range(min, max + 1);
            do
            {
                _number2 = Random.Range(min, max + 1);
            } while (_number1 == _number2);

            if (_pickNumber == null)
            {
                var canvas = GameObject.Find("Canvas");
                _pickNumber = Object.Instantiate(LevelBasicsManager.pickNumberPrefab, canvas.transform).GetComponent<PickNumber>();
            }
            _pickNumber.Init(_number1, _number2, OnElementClicked);
        }

        private void OnElementClicked(int number)
        {
            var smallerNumber = Mathf.Min(_number1, _number2);
            if (number == smallerNumber)
            {
                LevelBasicsManager.IncreaseScoreCount();
                InitTask();
            }
            else
            {
                LevelBasicsManager.IncreaseMistakeCount();
            }
        }

        public override string GetTaskAsString()
        {
            return $"{_number1} and {_number2}";
        }

        public override void DestroyTask()
        {
            if (_pickNumber == null)
            {
                return;
            }
            Object.Destroy(_pickNumber.gameObject);
            _pickNumber = null;
        }
    }
}