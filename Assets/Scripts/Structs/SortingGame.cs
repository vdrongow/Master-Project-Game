using Enums;

namespace Structs
{
    public struct SortingGame
    {
        public ESortingAlgorithm SortingAlgorithm { get; }
        public ESortType SortType { get; }
        public int ArraySize { get; }
        public int MistakeCount { get; set; }
        public bool IsRunning { get; set; }

        public SortingGame(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize, int mistakeCount = 0, bool isRunning = false)
        {
            SortingAlgorithm = sortingAlgorithm;
            SortType = sortType;
            ArraySize = arraySize;
            MistakeCount = mistakeCount;
            IsRunning = isRunning;
        }
    
        public override string ToString() => $"Sorting Algorithm: {SortingAlgorithm}, \n" +
                                             $"Sort Type: {SortType}, \n" +
                                             $"Array Size: {ArraySize}, \n" +
                                             $"Mistake Count: {MistakeCount}";
    }
}