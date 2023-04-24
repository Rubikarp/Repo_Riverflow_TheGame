using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace RiverFlow.Core
{

    [System.Serializable]
    public struct River
    {
        [Header("Data"), SerializeField]
        private List<Vector2Int> riverTiles;
        public List<Vector2Int> tiles
        {
            get => riverTiles;
            set => riverTiles = value;
        }
        public Vector2Int startNode => riverTiles.First();
        public Vector2Int endNode => riverTiles.Last();
        public int Lenght => riverTiles.Count;

        public void Reverse() { riverTiles.Reverse(); }

        public River(List<Vector2Int> tiles) 
        { riverTiles = tiles; }
        public River(River canal) 
        { riverTiles = canal.riverTiles; }
        public River(Vector2Int startNode, Vector2Int endNode) 
        { riverTiles = new List<Vector2Int>() { startNode, endNode }; }
    }
}