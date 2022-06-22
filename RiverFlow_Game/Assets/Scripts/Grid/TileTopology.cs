using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileTopology
    {
        [Header("State")]
        public TileType type;
        public FlowStrenght flow;

    }
}
