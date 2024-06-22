using UnityEngine;

public class ArrayElement : MonoBehaviour
{
    public int Value { get; set; }
    public bool IsHighlighted { get; set; }
    public bool IsSorted { get; set; }
    
    public GameObject highlightCurrent = null!;
    
    public void Init(int value)
    {
        Value = value;
        IsHighlighted = false;
        IsSorted = false;
    }

    public override string ToString()
    {
        return "Value: " + Value + ", IsHighlighted: " + IsHighlighted + ", IsSorted: " + IsSorted;
    }
}