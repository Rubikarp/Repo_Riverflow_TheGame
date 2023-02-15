using UnityEngine;

namespace RiverFlow.Core
{
    public abstract class TileObject : MonoBehaviour
    {
        [Header("TileObject Value")]
        public Vector2Int gridPos;
        public readonly TileData[] tilesOn = new TileData[1];

        public TileData tileOn => tilesOn[0];

        public TileObject(int tileSize = 1)
        {
            tilesOn = new TileData[tileSize];
        }

        public virtual void LinkToTile(TileData tile)
        {
            tilesOn[0] = tile;
        }
        public virtual void UnlinkTile()
        {
            tilesOn[0] = null;
        }
    }
}
