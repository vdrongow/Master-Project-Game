using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class SortTypeToggle : MonoBehaviour
    {
        private ToggleGroup toggleGroup;
        private List<Toggle> toggles;

        private void Start()
        {
            toggleGroup = GetComponent<ToggleGroup>();
            toggles = new List<Toggle>(toggleGroup.GetComponentsInChildren<Toggle>());

            foreach (var toggle in toggles)
            {
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                var activeToggleIndex = GetActiveToggleIndex();
                Debug.Log("Active Toggle Index: " + activeToggleIndex);
            }
        }

        private int GetActiveToggleIndex()
        {
            for (var i = 0; i < toggles.Count; i++)
            {
                if (toggles[i].isOn)
                {
                    return i;
                }
            }
            return -1; // Return -1 if no toggle is active
        }
        
        public ESortType GetSortType() => (ESortType)GetActiveToggleIndex();
    }
}