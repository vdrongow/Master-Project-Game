using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
// ReSharper disable MemberCanBePrivate.Global

/// <summary>
///  this class is used to create and visualise an array of integers
/// </summary>
public class ArrayView
{
    public List<ArrayElement> ArrayElements = new();
    public List<ArrayElement> SortedArrayElements = new();
    public readonly int ArraySize;
    public ESortType SortType;

    private GameObject _arrayParent;

    private ArraySettings _arraySettings;

    public ArrayView(
        GameObject arrayParent,
        int arraySize,
        ArraySettings arraySettings,
        ESortType sortType = ESortType.Unsorted)
    {
        _arrayParent = arrayParent;
        ArraySize = arraySize;
        _arraySettings = arraySettings;
        SortType = sortType;
        InitArray();
    }
    
    private void InitArray()
    {
        // if values in the array should be unique, make sure the max value is greater than the possible options
        if (_arraySettings.uniqueValues)
        {
            _arraySettings.maxValue = _arraySettings.minValue + ArraySize - 1;
        }
        
        // create an array of random integers
        var array = new int[ArraySize];
        for (var i = 0; i < ArraySize; i++)
        {
            int value;
            if(_arraySettings.uniqueValues)
            {
                do
                {
                    value = Random.Range(_arraySettings.minValue, _arraySettings.maxValue + 1);
                } while (Array.IndexOf(array, value) != -1);
            }
            else
            {
                value = Random.Range(_arraySettings.minValue, _arraySettings.maxValue + 1);
            }
            array[i] = value;
        }
        
        // sort the array according to the sort type
        switch (SortType)
        {
            case ESortType.Sorted:
                Array.Sort(array);
                break;
            case ESortType.ReverseSorted:
                Array.Sort(array);
                Array.Reverse(array);
                break;
            case ESortType.AllTheSame:
                var value = Random.Range(_arraySettings.minValue, _arraySettings.maxValue);
                for (var i = 0; i < ArraySize; i++)
                {
                    array[i] = value;
                }
                break;
            case ESortType.Unsorted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // visualise the array using bars with different heights according to the values
        for (var i = 0; i < ArraySize; i++)
        {
            var go = Object.Instantiate(_arraySettings.barPrefab, _arrayParent.transform);
            var barTransform = go.GetComponent<RectTransform>();
            // safe the current height of the bar as 100%
            var sizeDelta = barTransform.sizeDelta;
            var maxBarHeight = sizeDelta.y;
            // scale the bar height according to the value where 100% is the maxBarHeight and 0% is 0
            var barHeight = maxBarHeight * array[i] / _arraySettings.maxValue;
            // set the new height of the bar
            barTransform.sizeDelta = new Vector2(sizeDelta.x, barHeight);
            go.name = $"ArrayObject{i}";

            var arrayElement = go.GetComponent<ArrayElement>();
            arrayElement.Init(array[i]);
            ArrayElements.Add(arrayElement);
        }
        
        // sort the array to compare the values with the sorted array
        SortArray();
        foreach (var arrayElement in ArrayElements)
        {
            // set the sorted flag for each element
            arrayElement.IsSorted = IsElementSorted(ArrayElements.IndexOf(arrayElement));
        }
    }

    public void HighlightElement(int index)
    {
        var barRenderer = ArrayElements[index].transform.GetComponent<Image>();
        barRenderer.color = _arraySettings.highlightColor;
        ArrayElements[index].IsHighlighted = true;
    }
    
    public void UnhighlightElement(int index)
    {
        var barRenderer = ArrayElements[index].transform.GetComponent<Image>();
        barRenderer.color = _arraySettings.defaultColor;
        ArrayElements[index].IsHighlighted = false;
    }
    
    public void ClearHighlights()
    {
        foreach (var arrayElement in ArrayElements)
        {
            var barRenderer = arrayElement.transform.GetComponent<Image>();
            barRenderer.color = _arraySettings.defaultColor;
            arrayElement.IsHighlighted = false;
        }
    }
    
    public bool IsSorted()
    {
        for (var i = 0; i < ArraySize - 1; i++)
        {
            if (ArrayElements[i].Value > ArrayElements[i + 1].Value)
            {
                return false;
            }
        }
        return true;
    }
    
    public void SortArray()
    {
        var array = ArrayElements.ToArray();
        Array.Sort(array, (a, b) => a.Value.CompareTo(b.Value));
        
        SortedArrayElements = array.ToList();
    }

    public bool IsElementSorted(int index)
    {
        var arrayElement = ArrayElements[index];
        var wantedIndex = SortedArrayElements.IndexOf(arrayElement);
        return index == wantedIndex;
    }
    
    public bool IsEmpty => ArrayElements.Count == 0;

    public void DestroyArray()
    {
        foreach (var arrayElement in ArrayElements)
        {
            Object.Destroy(arrayElement.gameObject);
        }
    }
}