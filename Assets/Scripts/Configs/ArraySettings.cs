using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ArraySettings", menuName = "Configs/ArraySettings", order = 2)]
    public sealed class ArraySettings : ScriptableObject
    {
        [Header("Prefabs")]
        public GameObject barPrefab = null!;

        [Space]
        public float maxBarHeight = 805f;
        
        [Header("Values")]
        public int minValue = 1;
        public int maxValue = 10;
        
        public bool uniqueValues = false;
        
        [Header("Array")]
        public int minArraySize = 5;
        public int maxArraySize = 20;
        
        [Header("Sorting")]
        public float sortingSpeed = 1f;
        public float errorCooldown = 1f;

        [Header("Colors")]
        public Color highlightColor = Color.blue;
        public Color defaultColor = Color.white;
        public Color sortedColor = Color.grey;
    }
}