using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow.Core
{
    public class LevelInventory : MonoBehaviour
    {
        [Header("Pelle")]
        public int digAmmount = 20;

        [Header("Item")]
        public int lakesAmmount = 0;
        public int cloudsAmmount = 0;
        public int sourcesAmmount = 0;
        public int tunnelsAmmount = 0;

        public UnityEvent OnInventoryChange;

        private void Update()
        {
            OnInventoryChange.Invoke();
        }
    }
}
