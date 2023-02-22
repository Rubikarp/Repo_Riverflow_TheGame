using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class DataGrid<T> where T : new()
    {
        public Vector2Int Size { get => size; set => size = value; }
        [SerializeField] private Vector2Int size = new Vector2Int(8, 8);
        
        [HideInInspector] private T[] tiles = new T[8 * 8];
        public T GetData(Vector2Int pos) => GetData(pos.x, pos.y);
        public T GetData(int x, int y) => tiles[x + (size.y - 1 - y) * size.x];

        public DataGrid(Vector2Int size)
        {
            Size = size;
            tiles = new T[Size.x * Size.y];
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    tiles[x + (size.y - 1 - y) * size.x] = new T();
                }
            }
        }
    }

}
