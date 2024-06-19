using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public bool devMode = false;
        public bool showDebugLogs = true;
        
        [Space]
        public int countdownTime = 3;
    }
}