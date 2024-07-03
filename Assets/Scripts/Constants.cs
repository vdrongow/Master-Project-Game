// ReSharper disable InconsistentNaming
public static class Constants
{
    #region Scene Names

    public static string LOGIN_SCENE = "Login";
    public static string MAIN_MENU_SCENE = "MainMenu";
    public static string LEVEL_SORTING_SCENE = "LevelSorting";
    public static string LEVEL_BASICS_SCENE = "LevelBasics";

    #endregion

    #region Adlete Activity Names

    public static string ACTIVITY_BUBBLE_SORT_FINISHED = "activityFinishedBubbleSort";
    public static string ACTIVITY_BUBBLE_SORT_SWAP_ELEMENTS = "activitySwapElements";
    public static string ACTIVITY_BUBBLE_SORT_STEP_OVER = "activityStepOver";
    
    public static string ACTIVITY_SELECTION_SORT_FINISHED = "activityFinishedSelectionSort";
    public static string ACTIVITY_SELECTION_SORT_FOUND_NEW_MIN = "activityFoundNewMin";
    public static string ACTIVITY_SELECTION_SORT_NO_NEW_MIN = "activityNoNewMin";
    
    public static string ACTIVITY_INSERTION_SORT_FINISHED = "activityFinishedInsertionSort";
    public static string ACTIVITY_INSERTION_SORT_SWAP_FURTHER_FORWARDS = "activitySwapFurtherForwards";
    public static string ACTIVITY_INSERTION_SORT_INSERT_ELEMENT = "activityInsertElement";
    
    public static string ACTIVITY_IDENTIFY_SMALLEST_ELEMENT = "activityIdentifySmallestElement";
    public static string ACTIVITY_IDENTIFY_LARGEST_ELEMENT = "activityIdentifyLargestElement";
    public static string ACTIVITY_IDENTIFY_SMALLER_NUMBER = "activityIdentifySmallerNumber";
    public static string ACTIVITY_IDENTIFY_LARGER_NUMBER = "activityIdentifyLargerNumber";

    #endregion
    
    #region Lobby Variables

    public const string LOBBY_IS_GAME_STARTED = "LobbyIsGameStarted";
    public const string LOBBY_IS_GAME_PAUSED = "LobbyIsGamePaused";
    
    public const string PLAYER_NAME = "PlayerName";
    public const string PLAYER_FINISHED_LEVELS = "PlayerFinishedLevels";
    public const string PLAYER_TOTAL_MISTAKES = "PlayerTotalMistakes";
    public const string PLAYER_TOTAL_PLAYED_TIME = "PlayerTotalPlayedTime";

    #endregion
}