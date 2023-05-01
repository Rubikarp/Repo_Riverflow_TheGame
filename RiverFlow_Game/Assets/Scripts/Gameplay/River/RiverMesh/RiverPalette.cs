using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class RiverPalette : SingletonSCO<RiverPalette>
    {
        [Header("River Palette")]
        [ColorUsage(true, false)] public Color color_00 = Color.blue;
        [ColorUsage(true, false)] public Color color_25 = Color.blue;
        [ColorUsage(true, false)] public Color color_50 = Color.blue;
        [ColorUsage(true, false)] public Color color_75 = Color.blue;
        [ColorUsage(true, false)] public Color color_100 = Color.blue;

        [Header("BackUp")]
        [ColorUsage(true, false)] public Color errorMat = Color.magenta;

        public Color FromIrrig(FlowStrenght irrigation)
        {
            switch (irrigation)
            {
                case FlowStrenght._00_: return color_00;
                case FlowStrenght._25_: return color_25;
                case FlowStrenght._50_: return color_50;
                case FlowStrenght._75_: return color_75;
                case FlowStrenght._100_: return color_100;
                default: return errorMat;
            }
        }
    }
}
