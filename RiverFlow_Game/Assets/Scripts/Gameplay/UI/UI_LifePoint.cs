using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    public class UI_LifePoint : MonoBehaviour
    {
        public const int MAX_LIFE = 3;

        [field: SerializeField] //, ProgressBar(MAX_LIFE)] 
        public int life { get; private set; } = 3;
        
        [SerializeField] private Image[] lifePoints;

        public void Update()
        {
            for (int i = 0; i < lifePoints.Length; i++)
            {
                lifePoints[i].gameObject.SetActive(i <= life);
            }
        }
    }
}
