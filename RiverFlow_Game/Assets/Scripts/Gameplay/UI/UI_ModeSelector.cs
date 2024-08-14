using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiverFlow.Core
{
    public class UI_ModeSelector : MonoBehaviour
    {
        [SerializeField]
        private Button modeBtn;
        [SerializeField]
        private TextMeshProUGUI inventoryCount;

        public void RefreshCount(int pQuantity)
        {
            inventoryCount.text = pQuantity.ToString();
        }
        public void RefreshCount(string pText)
        {
            inventoryCount.text = pText;
        }
    }
}
