using System;
using System.Text;
using Configs;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
///  this class is used to create and visualise an array of integers
/// </summary>
public class ArrayView
{
    public int[] array;
    public readonly int arraySize;
    public ESortType sortType;

    private GameObject arrayParent;
    private GameObject[] arrayObjects;
    
    private ArraySettings arraySettings;

    public ArrayView(
        GameObject arrayParent,
        int arraySize,
        ArraySettings arraySettings,
        ESortType sortType = ESortType.Unsorted)
    {
        this.arrayParent = arrayParent;
        this.arraySize = arraySize;
        this.arraySettings = arraySettings;
        this.sortType = sortType;
        InitArray();
    }
    
    private void InitArray()
    {
        // if values in the array should be unique, make sure the max value is greater than the possible options
        if (arraySettings.uniqueValues)
        {
            arraySettings.maxValue = arraySettings.minValue + arraySize - 1;
        }
        
        // create an array of random integers
        array = new int[arraySize];
        for (var i = 0; i < arraySize; i++)
        {
            int value;
            if(arraySettings.uniqueValues)
            {
                do
                {
                    value = Random.Range(arraySettings.minValue, arraySettings.maxValue + 1);
                } while (Array.IndexOf(array, value) != -1);
            }
            else
            {
                value = Random.Range(arraySettings.minValue, arraySettings.maxValue + 1);
            }
            array[i] = value;
        }
        
        // sort the array according to the sort type
        switch (sortType)
        {
            case ESortType.Sorted:
                Array.Sort(array);
                break;
            case ESortType.ReverseSorted:
                Array.Sort(array);
                Array.Reverse(array);
                break;
            case ESortType.AllTheSame:
                var value = Random.Range(arraySettings.minValue, arraySettings.maxValue);
                for (var i = 0; i < arraySize; i++)
                {
                    array[i] = value;
                }
                break;
            case ESortType.Unsorted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        arrayObjects = new GameObject[arraySize];
        
        // visualise the array using bars with different heights according to the values
        for (var i = 0; i < arraySize; i++)
        {
            arrayObjects[i] = Object.Instantiate(arraySettings.barPrefab, arrayParent.transform);
            var barTransform = arrayObjects[i].GetComponent<RectTransform>();
            // safe the current height of the bar as 100%
            var sizeDelta = barTransform.sizeDelta;
            var maxBarHeight = sizeDelta.y;
            // scale the bar height according to the value where 100% is the maxBarHeight and 0% is 0
            var barHeight = maxBarHeight * array[i] / arraySettings.maxValue;
            // set the new height of the bar
            barTransform.sizeDelta = new Vector2(sizeDelta.x, barHeight);

            arrayObjects[i].name = $"ArrayObject{i}";
        }
    }

    public void HighlightElement(int index)
    {
        var barRenderer = arrayObjects[index].GetComponent<Image>();
        barRenderer.color = arraySettings.highlightColor;
    }
    
    public void UnhighlightElement(int index)
    {
        var barRenderer = arrayObjects[index].GetComponent<Image>();
        barRenderer.color = arraySettings.defaultColor;
    }
    
    public void ClearHighlights()
    {
        foreach (var arrayObject in arrayObjects)
        {
            var barRenderer = arrayObject.GetComponent<Image>();
            barRenderer.color = arraySettings.defaultColor;
        }
    }

    public string AsString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Array: ");
        sb.AppendLine($"Size: {arraySize.ToString()}");
        sb.AppendLine($"SortType: {sortType.AsString()}");
        sb.AppendLine(string.Join(", ", array));
        
        return sb.ToString();
    }
    
    public void DestroyArray()
    {
        foreach (var arrayObject in arrayObjects)
        {
            Object.Destroy(arrayObject);
        }
    }
}