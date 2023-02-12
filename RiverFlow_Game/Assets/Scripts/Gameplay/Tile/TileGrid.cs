using UnityEngine;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileGrid
    {
        public Vector2Int Size { get => size; set => size = value; }
        [SerializeField] private Vector2Int size = new Vector2Int(8, 8);
        
        [HideInInspector] private TileData[] tiles = new TileData[8 * 8];
        public TileData GetTile(Vector2Int pos) => GetTile(pos.x, pos.y);
        public TileData GetTile(int x, int y) => tiles[x + (size.y - 1 - y) * size.x];

        public TileGrid(Vector2Int size)
        {
            Size = size;
            tiles = new TileData[Size.x * Size.y];
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    tiles[x + (size.y - 1 - y) * size.x] = new TileData();
                }
            }
        }

    }

}
