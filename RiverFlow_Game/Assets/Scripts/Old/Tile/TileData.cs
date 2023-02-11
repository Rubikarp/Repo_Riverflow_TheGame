using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileData
    {
        [Header("State")]
        public TileTopology topology = TileTopology.Grass;
        public FlowStrenght flow = FlowStrenght._00_;
        //Element
        //List<Canals>

    }
}

