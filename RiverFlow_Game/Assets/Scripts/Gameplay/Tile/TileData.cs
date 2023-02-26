using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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
        public FlowStrenght irrigation = FlowStrenght._00_;

        [Header("State")]
        public Element element;
        public Plant plant;
        public List<River> riverIn = new List<River>(2);
        public List<River> riverOut = new List<River>(2);

        public int LinkAmount => riverIn.Count + riverOut.Count;

    }
}

