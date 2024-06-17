using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class SortTypeToggle : MonoBehaviour
    {
        public ESortType GetSortType()
        {
            var toggleGroup = GetComponent<ToggleGroup>();
            var activeToggleIndex = 0;
            // get the child index of the active toggle
            var current = toggleGroup.ActiveToggles().GetEnumerator().Current;
            if (current != null)
            {
                activeToggleIndex = current.transform.GetSiblingIndex();
            }
            return (ESortType)activeToggleIndex;
        }
    }
}