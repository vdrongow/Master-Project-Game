using Enums;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public bool devMode = false;
        public bool showDebugLogs = true;
        
        [Header("Countdown")]
        public bool showCountdown = true;
        public int countdownTime = 3;
        
        [Header("Timer")]
        public bool showTimer = true;
        public int timeLimit = 60;
        
        [Header("Sorting Algorithm Level")]
        public ESortingAlgorithm defaultSortingAlgorithm = ESortingAlgorithm.BubbleSort;
        public ESortType defaultSortType = ESortType.Unsorted;
        public int defaultArraySize = 5;
        
        [Header("Basic Skill Level")]
        public EBasicSkill defaultBasicSkill = EBasicSkill.IdentifyLargerNumber;
        public int defaultBasicLevelElements = 5;
    }
}