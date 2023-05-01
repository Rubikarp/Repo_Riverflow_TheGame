using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    [RequireComponent(typeof(RiverMesh))]
    public class River : MonoBehaviour
    {
        private RiverMesh riverMesh;

        [Header("Data"), SerializeField]
        private List<Vector2Int> riverTiles = new List<Vector2Int>();
        public List<Vector2Int> tiles
        {
            get => riverTiles;
            set => riverTiles = value;
        }
        public Vector2Int startNode => riverTiles.First();
        public Vector2Int endNode => riverTiles.Last();
        public int Lenght => riverTiles.Count;

        public void Reverse() { riverTiles.Reverse(); }


        public void Initialised(TileData startNode, TileData endNode) 
            => Initialised(new List<TileData>() { startNode, endNode });
        public void Initialised(List<TileData> tiles)
        {
            riverTiles = tiles.Select(x=> x.gridPos).ToList();

            riverMesh = GetComponent<RiverMesh>();
            var points = tiles.Select(x => new RiverPoint(x.gridPos)).ToList();


            //riverMesh.SetPoints(tiles);
        }
    }
}