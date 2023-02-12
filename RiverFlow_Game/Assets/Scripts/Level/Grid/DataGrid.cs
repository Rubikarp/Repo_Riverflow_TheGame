using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class DataGrid<T> where T : new()
    {
        [Header("Data")]
        [SerializeField] protected T[,] tiles;

        //Accesseurs
        public T[,] Tiles { get => tiles; }
        public Vector2Int Size
        {
            get => new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));
            private set => tiles = new T[value.x, value.y];
        }

        //Constructeur
        public DataGrid(Vector2Int size)=> Initialize(size);

        public void Initialize(Vector2Int size)
        {
            Size = size;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    tiles[x, y] = new T();                  
        }
        public void Initialize(T[,] copy)
        {
            Size = new Vector2Int(copy.GetLength(0), copy.GetLength(1));

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    tiles[x, y] = copy[x,y];
                }
            }
        }
    }
}
