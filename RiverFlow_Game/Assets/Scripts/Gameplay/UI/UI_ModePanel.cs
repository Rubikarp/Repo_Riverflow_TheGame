using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace RiverFlow.Core
{
    public class UI_ModePanel : MonoBehaviour
    {
        [SerializeField]
        private LevelInventory inventory;
        [Space(10)]
        [SerializeField]
        private CanvasGroup infoPanel;

        [Space(10)]
        [SerializeField]
        private UI_ModeSelector shovelSelection, eraserSelection, lakeSelection, cloudSelection, sourceSelection, tunnelSelection;

        private void OnEnable()
        {
            inventory.OnInventoryChange.AddListener(RefreshInventoryCount);
        }
        private void OnDisable()
        {
            inventory.OnInventoryChange.RemoveListener(RefreshInventoryCount);
        }
        private void RefreshInventoryCount()
        {
            shovelSelection.RefreshCount(inventory.digAmmount);
            eraserSelection.RefreshCount("ထ");

            lakeSelection.RefreshCount(inventory.lakesAmmount);
            cloudSelection.RefreshCount(inventory.cloudsAmmount);
            sourceSelection.RefreshCount(inventory.sourcesAmmount);

            tunnelSelection.RefreshCount(inventory.tunnelsAmmount);
        }


        void Update()
        {
        
        }
    }
}
