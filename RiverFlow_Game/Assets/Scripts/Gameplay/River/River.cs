using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using NaughtyAttributes;
using System;

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

        public void Reverse() 
        {
            this.UnlinkToGrid();
            riverTiles.Reverse();
            this.LinkToGrid();
        }


        [Button]
        public void ReInitialised()
        {
            riverTiles = tiles;
            riverMesh = GetComponent<RiverMesh>();
            riverMesh.SetPoints(RiverExtension.River2Point(riverTiles));
        }
        public void Initialise(Vector2Int startNode, Vector2Int endNode) => Initialise(new List<Vector2Int>() { startNode, endNode });
        public void Initialise(List<Vector2Int> tiles)
        {
            riverTiles = tiles;
            riverMesh = GetComponent<RiverMesh>();
            riverMesh.SetPoints(RiverExtension.River2Point(riverTiles));
        }

        public void Extend(List<Vector2Int> tiles, Vector2Int addedTile) => Initialise(tiles.Append(addedTile).ToList());
        public void Extend(List<Vector2Int> tiles, List<Vector2Int> addedTiles) => Initialise(tiles.Union(addedTiles).ToList());


    }
}