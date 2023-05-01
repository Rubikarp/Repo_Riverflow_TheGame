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

    }
}
