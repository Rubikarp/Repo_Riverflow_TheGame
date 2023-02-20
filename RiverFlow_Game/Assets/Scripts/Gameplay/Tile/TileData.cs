using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileData
    {
        [Header("Param")]
        public Vector2Int gridPos;
        //[HideInInspector] public TileData[] neighboor = new TileData[8];

        [Header("State")]
        public TileTopology topology = TileTopology.Grass;
        public FlowStrenght flow = FlowStrenght._00_;

        [Header("State")]
        public Element element;
        //public List<Canal>

    }
}

