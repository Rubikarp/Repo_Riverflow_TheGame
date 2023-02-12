using UnityEngine;

namespace RiverFlow.Core
{
    public abstract class Element : MonoBehaviour
    {
        [Header("Element Value")]
        public Vector2Int gridPos;
        public TileData currentTile;

        [Header("Element Parameter")]
        public FlowStrenght irrigationLvl = FlowStrenght._100_;

        public virtual void LinkToTile(TileData tile)
        {
            currentTile = tile;
            currentTile.element = this;
        }
        public virtual void UnLinkToTile()
        {
            currentTile.element = null;
            currentTile = null;
        }
    }

    public class Cloud : Element
    {
        public Cloud()
        {
            irrigationLvl = FlowStrenght._25_;
        }


    }
}
