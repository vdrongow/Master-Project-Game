using Enums;

public class Game
{
    public ESortingAlgorithm SortingAlgorithm;
    public ESortType SortType;
    public int ArraySize;
    
    public Game(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
    {
        SortingAlgorithm = sortingAlgorithm;
        SortType = sortType;
        ArraySize = arraySize;
    }
}