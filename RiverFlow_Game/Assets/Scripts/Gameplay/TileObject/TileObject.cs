using UnityEngine;

namespace RiverFlow.Core
{
    public abstract class TileObject : MonoBehaviour
    {
        [Header("TileObject Value")]
        public Vector2Int gridPos;

        public TileObject(Vector2Int pos)
        {
            gridPos = pos;
        }
    }
}
