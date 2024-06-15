using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI handleText;
    [SerializeField]
    private TextMeshProUGUI minText;
    [SerializeField]
    private TextMeshProUGUI maxText;
    
    public void Init()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    
    private void OnSliderValueChanged(float value)
    {
        handleText.text = value.ToString();
    }
    
    public void SetMinValue(float value)
    {
        slider.minValue = value;
        minText.text = value.ToString();
    }
    
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        maxText.text = value.ToString();
    }
    
    public void SetValue(float value)
    {
        slider.value = value;
        handleText.text = value.ToString();
    }
    
    public float GetValue()
    {
        return slider.value;
    }
}