using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ArraySizeSlider : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI handleText;
        [SerializeField]
        private TextMeshProUGUI minText;
        [SerializeField]
        private TextMeshProUGUI maxText;
        
        private Slider _slider;
    
        private void Start()
        {
            var gameManager = GameManager.Singleton;
            
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);

            SetMinValue(gameManager.arraySettings.minArraySize);
            SetMaxValue(gameManager.arraySettings.maxArraySize);
            var handleValue = (gameManager.arraySettings.minArraySize + gameManager.arraySettings.maxArraySize) / 2;
            SetValue(handleValue);
        }
    
        private void OnSliderValueChanged(float value)
        {
            handleText.text = value.ToString();
        }
    
        public void SetMinValue(float value)
        {
            _slider.minValue = value;
            minText.text = value.ToString();
        }
    
        public void SetMaxValue(float value)
        {
            _slider.maxValue = value;
            maxText.text = value.ToString();
        }
    
        public void SetValue(float value)
        {
            _slider.value = value;
            handleText.text = value.ToString();
        }
    
        public float GetValue()
        {
            return _slider.value;
        }
    }
}