using UnityEngine;

namespace RiverFlow.Core
{
    public class TileTopologyPalette : SingletonSCO<TileTopologyPalette>
    {
        [Header("Ground")]
        [ColorUsage(true, false)] public Color holedGround = Color.gray;
        [Space(5)]
        [ColorUsage(true, false)] public Color grass = Color.green;
        [ColorUsage(true, false)] public Color clay = Color.red;
        [ColorUsage(true, false)] public Color aride = Color.yellow;
        [ColorUsage(true, false)] public Color mountain = Color.gray;

        [Header("BackUp")]
        [ColorUsage(true, false)] public Color errorMat = Color.magenta;

        public Color FromTopo(TileTopology topo)
        {
            switch (topo)
            {
                case TileTopology.Grass:
                    return grass;
                case TileTopology.Clay:
                    return clay;
                case TileTopology.Sand:
                    return aride;
                case TileTopology.Mountain:
                    return mountain;
                case TileTopology.None:
                    return errorMat;
                default:
                    return errorMat;
            }
        }
    }
}
