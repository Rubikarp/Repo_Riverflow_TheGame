using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileTopology
    {
        [Header("State")]
        public TileType type = TileType.Grass;
        public FlowStrenght flow = FlowStrenght._00_;
    }

    public enum TileType
    {
        None = 0,
        Grass = 1,
        Clay = 2,
        Sand = 3,
        Mountain = 4,
    }
    public enum FlowStrenght
    {
        _00_ = 0,
        _25_ = 1,
        _50_ = 2,
        _75_ = 3,
        _100_ = 4
    }
}

