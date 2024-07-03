using System;
using Enums;

public class Activity
{
    public EActivityType ActivityType;
    public int CorrectAnswers;
    public int WrongAnswers;
    public int TotalTasks;
    
    public Activity(EActivityType activityType)
    {
        ActivityType = activityType;
        CorrectAnswers = 0;
        WrongAnswers = 0;
        TotalTasks = 0;
    }
    
    public void AddTaskInput(int correctness)
    {
        switch (correctness)
        {
            case 1:
                CorrectAnswers++;
                break;
            case 0:
                WrongAnswers++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(correctness), correctness, null);
        }
        TotalTasks++;
    }
    
    public void ResetActivity()
    {
        CorrectAnswers = 0;
        WrongAnswers = 0;
        TotalTasks = 0;
    }

    public float GetTotalCorrectness()
    {
        // calculate the correctness of the activity from 0 to 1, e.g.: 6 correct answers out of 10 tasks = 0.6
        return (float) CorrectAnswers / TotalTasks;
    }
    
    public string GetActivityName()
    {
        return ActivityType switch
        {
            EActivityType.None => "None",
            EActivityType.BubbleSortSwapElements => Constants.ACTIVITY_BUBBLE_SORT_SWAP_ELEMENTS,
            EActivityType.BubbleSortStepOver => Constants.ACTIVITY_BUBBLE_SORT_STEP_OVER,
            EActivityType.SelectionSortFoundNewMin => Constants.ACTIVITY_SELECTION_SORT_FOUND_NEW_MIN,
            EActivityType.SelectionSortNoNewMin => Constants.ACTIVITY_SELECTION_SORT_NO_NEW_MIN,
            EActivityType.InsertionSortSwapFurtherForwards => Constants.ACTIVITY_INSERTION_SORT_SWAP_FURTHER_FORWARDS,
            EActivityType.InsertionSortInsertElement => Constants.ACTIVITY_INSERTION_SORT_INSERT_ELEMENT,
            EActivityType.IdentifySmallestElement => Constants.ACTIVITY_IDENTIFY_SMALLEST_ELEMENT,
            EActivityType.IdentifyLargestElement => Constants.ACTIVITY_IDENTIFY_LARGEST_ELEMENT,
            EActivityType.IdentifySmallerNumber => Constants.ACTIVITY_IDENTIFY_SMALLER_NUMBER,
            EActivityType.IdentifyLargerNumber => Constants.ACTIVITY_IDENTIFY_LARGER_NUMBER,
            _ => throw new ArgumentOutOfRangeException(nameof(ActivityType), ActivityType, null)
        };
    }
}