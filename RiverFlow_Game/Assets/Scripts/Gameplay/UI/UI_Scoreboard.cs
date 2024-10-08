using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    public class UI_Scoreboard : MonoBehaviour
    {
        public TextMeshProUGUI scoreArea;
        [Space]
        public int score;

        private void Update()
        {
            scoreArea.text = score.ToString();
        }
    }
}
