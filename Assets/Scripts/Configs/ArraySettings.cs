using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ArraySettings", menuName = "Configs/ArraySettings", order = 2)]
    public sealed class ArraySettings : ScriptableObject
    {
        public GameObject barPrefab = null!;
        
        public int minValue = 1;
        public int maxValue = 10;
        
        public int minArraySize = 5;
        public int maxArraySize = 20;
        
        public bool uniqueValues = false;

        public Color highlightColor = Color.blue;
        public Color defaultColor = Color.white;
        public Color sortedColor = Color.grey;
    }
}