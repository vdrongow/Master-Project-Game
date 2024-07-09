using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrayElement : MonoBehaviour, IPointerClickHandler
{
    public int Value { get; set; }
    public bool IsHighlighted { get; set; }
    public bool IsSorted { get; set; }
    
    public GameObject highlightCurrent = null!;
    public TextMeshProUGUI valueText = null!;
    private Action _onClick = null!;

    public void Init(int value, Action onClick = null)
    {
        Value = value;
        IsHighlighted = false;
        IsSorted = false;
        highlightCurrent.SetActive(false);
        valueText.text = value.ToString();
        _onClick = onClick;
        SetBarHeight(value);
    }

    private void SetBarHeight(int value)
    {
        var arraySettings = GameManager.Singleton.arraySettings;
        var barTransform = GetComponent<RectTransform>();
        // safe the current height of the bar as 100%
        var delta = barTransform.sizeDelta;
        var sizeDelta = delta;
        // scale the bar height according to the value where 100% is the maxBarHeight and 0% is 0
        var barHeight = arraySettings.maxBarHeight * value / arraySettings.maxValue;
        // set the new height of the bar
        delta = new Vector2(sizeDelta.x, barHeight);
        barTransform.sizeDelta = delta;
        gameObject.name = $"ArrayElement{value}";
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke();
    }

    public override string ToString()
    {
        return "Value: " + Value + ", IsHighlighted: " + IsHighlighted + ", IsSorted: " + IsSorted;
    }
}