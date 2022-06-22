using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class DataGrid<T> //: MonoBehaviour
    {
        [SerializeField] Vector2Int size = new Vector2Int(16, 16);
        public Vector2Int Size { get => size; private set => size = value; }

        [Header("Data")]
        [SerializeField] protected T[,] tiles;
        public T[,] Tiles { get => tiles; }

        public void Initialize(Vector2Int size)
        {
            Size = size;
            tiles = new T[Size.x,Size.y];
        }

        /*
        #region Grid-Tile Methodes
        public T GetTile(int x, int y) => tiles[x + (y * (size.x))];
        public T GetTile(Vector2Int pos) => GetTile(pos.x, pos.y);
        public void SetTile(int x, int y, T value) =>  tiles[x + (y * (size.x))] = value;
        public void SetTile(Vector2Int pos, T value) => SetTile(pos.x, pos.y, value);
        #endregion
        */
    }
}
