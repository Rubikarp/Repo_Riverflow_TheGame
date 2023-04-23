using UnityEngine;

namespace RiverFlow.Core
{
    public class TopologyPalette : SingletonSCO<TopologyPalette>
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

        public Color FromTopo(Topology topo)
        {
            switch (topo)
            {
                case Topology.Grass:
                    return grass;
                case Topology.Clay:
                    return clay;
                case Topology.Sand:
                    return aride;
                case Topology.Mountain:
                    return mountain;
                case Topology.None:
                    return errorMat;
                default:
                    return errorMat;
            }
        }
    }
}
