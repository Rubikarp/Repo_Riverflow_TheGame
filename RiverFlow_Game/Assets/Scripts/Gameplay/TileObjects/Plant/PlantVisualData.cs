using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class PlantVisualData : SingletonSCO<PlantVisualData>
    {
        [Header("Grass"), Expandable]
        public PlantType grass_plantGrowth;
        [Header("Clay"), Expandable]
        public PlantType clay_plantGrowth;
        [Header("Sand"), Expandable]
        public PlantType sand_plantGrowth;

        public Sprite GetSprite(PlantState state, Topology type)
        {
            switch (type)
            {
                case Topology.Grass:
                    return grass_plantGrowth.StateSprite(state);
                case Topology.Clay:
                    return clay_plantGrowth.StateSprite(state);
                case Topology.Sand:
                    return sand_plantGrowth.StateSprite(state);
                default:
                    return null;
            }
        }
    }
}
