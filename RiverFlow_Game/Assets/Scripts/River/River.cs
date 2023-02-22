using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace RiverFlow.Core
{

    [System.Serializable]
    public class River
    {
        [Header("Data"), SerializeField]
        private List<RiverData> riverTiles;
        public List<RiverData> Tiles
        {
            get => riverTiles;
            set
            {
                riverTiles = value;
                onChange?.Invoke();
            }
        }
        public RiverData startNode => riverTiles.First();
        public RiverData endNode => riverTiles.Last();
        public int Lenght => riverTiles.Count;

        [Header("LinkedRiver")]
        public List<RiverData> aval;
        public List<RiverData> amont;

        [Header("Event")]
        public UnityEvent onChange;

        public River(Vector2Int startNode, Vector2Int endNode)
        {
            Tiles = new List<RiverData>() { new RiverData(startNode), new RiverData(endNode) };
        }
        public River(List<Vector2Int> tiles)
        {
            Tiles = tiles.Select(tile => new RiverData(tile)).ToList();
        }
        public River(River canal)
        {
            Tiles = new List<RiverData>(canal.riverTiles);
        }
    }

    [System.Serializable]
    public class RiverData
    {
        public Vector2Int tile;
        public FlowStrenght flow;

        public RiverData(Vector2Int tile, FlowStrenght flow = FlowStrenght._00_)
        {
            this.tile = tile;
            this.flow = flow;
        }
    }
}