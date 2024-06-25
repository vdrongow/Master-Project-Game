using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class OptionSelector : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI optionText;
        [SerializeField]
        private Button leftButton;
        [SerializeField]
        private Button rightButton;
        
        [SerializeField]
        private string[] options = { "Option 1", "Option 2", "Option 3", "Option 4" };
        private int _currentIndex;

        private void Start()
        {
            optionText.text = options[_currentIndex];

            leftButton.onClick.AddListener(OnLeftButtonClick);
            rightButton.onClick.AddListener(OnRightButtonClick);
        }

        public void OnLeftButtonClick()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = options.Length - 1;
            }

            optionText.text = options[_currentIndex];
        }

        public void OnRightButtonClick()
        {
            _currentIndex++;
            if (_currentIndex >= options.Length)
            {
                _currentIndex = 0;
            }

            optionText.text = options[_currentIndex];
        }
        
        public int GetCurrentIndex() => _currentIndex;
    }
}