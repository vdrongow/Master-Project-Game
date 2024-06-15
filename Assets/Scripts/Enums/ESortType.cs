using System;

namespace Enums
{
    [Serializable]
    public enum ESortType
    {
        Unsorted = 0,
        Sorted = 1,
        ReverseSorted = 2,
        AllTheSame = 3,
    }

    internal static class ESortTypeExtensions
    {
        public static string AsString(this ESortType sortType) => sortType switch
        {
            ESortType.Unsorted => nameof(ESortType.Unsorted),
            ESortType.Sorted => nameof(ESortType.Sorted),
            ESortType.ReverseSorted => nameof(ESortType.ReverseSorted),
            ESortType.AllTheSame => nameof(ESortType.AllTheSame),
            _ => throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null),
        };
    }
}