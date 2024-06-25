using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickNumber : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI number1Text;
    [SerializeField]
    private TextMeshProUGUI number2Text;
    [SerializeField]
    private Button number1Button;
    [SerializeField]
    private Button number2Button;
    
    public void Init(int number1, int number2, Action<int> onNumberClicked)
    {
        number1Text.text = number1.ToString();
        number2Text.text = number2.ToString();
        number1Button.onClick.AddListener(() => onNumberClicked(number1));
        number2Button.onClick.AddListener(() => onNumberClicked(number2));
    }
}