﻿using Enums;
using UnityEngine;
// ReSharper disable InconsistentNaming

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
        public int timeLimit = 60;
        
        [Header("MistakeVisualizer")]
        public float errorCooldown = 1f;
        
        [Header("Sorting Algorithm Level")]
        public ESortingAlgorithm defaultSortingAlgorithm = ESortingAlgorithm.BubbleSort;
        public ESortType defaultSortType = ESortType.Unsorted;
        public int defaultArraySize = 5;
        
        [Header("Basic Skill Level")]
        public EBasicSkill defaultBasicSkill = EBasicSkill.IdentifyLargerNumber;
        public int defaultBasicLevelElements = 5;
        public int defaultMinValue = 1;
        public int defaultMaxValue = 10;
        
        [Header("Adlete Configs")]
        public int adlete_requestInterval = 10;
        public float adlete_defaultDifficulty = 0.5f;
    }
}