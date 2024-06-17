using System;

namespace Enums
{
    [Serializable]
    public enum ESortingAlgorithm
    {
        BubbleSort = 0,
        SelectionSort = 1,
        InsertionSort = 2,
    }
    
    internal static class ESortingAlgorithmExtensions
    {
        public static string AsString(this ESortingAlgorithm sortingAlgorithm) => sortingAlgorithm switch
        {
            ESortingAlgorithm.BubbleSort => nameof(ESortingAlgorithm.BubbleSort),
            ESortingAlgorithm.SelectionSort => nameof(ESortingAlgorithm.SelectionSort),
            ESortingAlgorithm.InsertionSort => nameof(ESortingAlgorithm.InsertionSort),
            _ => throw new ArgumentOutOfRangeException(nameof(sortingAlgorithm), sortingAlgorithm, null),
        };
    }
}
