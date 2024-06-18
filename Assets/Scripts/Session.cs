using Adlete;
using Enums;

public class Session
{
    public Learner Learner;
    
    public ESortingAlgorithm SortingAlgorithm;
    public ESortType SortType;
    public int ArraySize;
    
    public Session(Learner learner, ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
    {
        Learner = learner;
        SortingAlgorithm = sortingAlgorithm;
        SortType = sortType;
        ArraySize = arraySize;
    }
}