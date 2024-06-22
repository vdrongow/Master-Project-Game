using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public bool devMode = false;
        public bool showDebugLogs = true;
        
        [Space]
        public bool showCountdown = true;
        public int countdownTime = 3;
        
        [Header("Sorting Algorithm Settings")]
        public ESortingAlgorithm defaultSortingAlgorithm = ESortingAlgorithm.BubbleSort;
        public ESortType defaultSortType = ESortType.Unsorted;
        public int defaultArraySize = 5;
    }
}