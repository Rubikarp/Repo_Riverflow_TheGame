using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RiverFlow.Core
{
    public class UI_Clock : ShaderLink
    {
        public Material mat;
        public DayTime dayTime;

        void Update()
        {
            mat.SetFloat("_DayTime", dayTime.progression / dayTime.dayDuration);
        }

        private void OnDisable()
        {
            mat.SetFloat("_DayTime", 1f);
        }
    }
}
