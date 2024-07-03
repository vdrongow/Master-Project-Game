using System;

namespace Enums
{
    [Serializable]
    public enum EActivityType
    {
        None = 0,
        BubbleSortSwapElements = 1,
        BubbleSortStepOver = 2,
        SelectionSortFoundNewMin = 3,
        SelectionSortNoNewMin = 4,
        InsertionSortSwapFurtherForwards = 5,
        InsertionSortInsertElement = 6,
        IdentifySmallestElement = 7,
        IdentifyLargestElement = 8,
        IdentifySmallerNumber = 9,
        IdentifyLargerNumber = 10,
    }
    
    public static class EActivityTypeExtensions
    {
        public static string AsString(this EActivityType activityType) => activityType switch
        {
            EActivityType.None => nameof(EActivityType.None),
            EActivityType.BubbleSortSwapElements => nameof(EActivityType.BubbleSortSwapElements),
            EActivityType.BubbleSortStepOver => nameof(EActivityType.BubbleSortStepOver),
            EActivityType.SelectionSortFoundNewMin => nameof(EActivityType.SelectionSortFoundNewMin),
            EActivityType.SelectionSortNoNewMin => nameof(EActivityType.SelectionSortNoNewMin),
            EActivityType.InsertionSortSwapFurtherForwards => nameof(EActivityType.InsertionSortSwapFurtherForwards),
            EActivityType.InsertionSortInsertElement => nameof(EActivityType.InsertionSortInsertElement),
            EActivityType.IdentifySmallestElement => nameof(EActivityType.IdentifySmallestElement),
            EActivityType.IdentifyLargestElement => nameof(EActivityType.IdentifyLargestElement),
            EActivityType.IdentifySmallerNumber => nameof(EActivityType.IdentifySmallerNumber),
            EActivityType.IdentifyLargerNumber => nameof(EActivityType.IdentifyLargerNumber),
            _ => throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null),
        };
    }
}