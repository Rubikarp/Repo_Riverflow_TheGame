using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    [System.Serializable]
    public struct TileData
    {
        [Header("Param")]
        public Vector2Int gridPos;

        [Header("State")]
        public Topology topology;
        public FlowStrenght irrigation;
        public FlowStrenght currentFlow;
        public FlowStrenght previousFlow;

        //[Header("State")]
        public Plant plant;
        public Element element;
        public List<Vector2Int> riverIn;
        public List<Vector2Int> riverOut;

        public int LinkAmount => riverOut.Count + riverIn.Count;
        public bool IsOccupied => !(plant is null) || !(element is null);

        public TileData(int x, int y)
        {
            gridPos = new Vector2Int(x,y);

            plant = null;
            element = null;

            topology = Topology.Grass;
            irrigation = FlowStrenght._00_;
            currentFlow = FlowStrenght._00_;
            previousFlow = FlowStrenght._00_;

            riverIn = new List<Vector2Int>(2);
            riverOut = new List<Vector2Int>(2);
        }
    }
}

