using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public string[] sceneNames =
        {
            "MainMenu",
            "Level",
        };
    }
}