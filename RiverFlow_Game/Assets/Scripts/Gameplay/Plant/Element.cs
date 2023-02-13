using System.Linq;
using UnityEngine;

namespace RiverFlow.Core
{
    public abstract class Element : MonoBehaviour
    {
        [Header("Element Value")]
        public Vector2Int gridPos;
        public readonly TileData[] tilesOn = new TileData[1];
        public readonly FlowStrenght irrigationLvl;

        public Element(FlowStrenght strenght = FlowStrenght._100_, int tileSize = 1)
        {
            irrigationLvl = strenght;
            tilesOn = new TileData[tileSize];
        }

        public virtual void LinkToTile(TileData tile)
        {
            tilesOn[0] = tile;
            tilesOn[0].element = this;
        }
        public virtual void UnlinkTile()
        {
            tilesOn[0] = null;
            tilesOn[0].element = null;
        }
    }

    public class Cloud : Element
    {
        public Cloud(): base(FlowStrenght._25_, 1) { }
    }
    public class Lake : Element
    {
        public Lake() : base(FlowStrenght._100_, 3) { }

        public void LinkToTile(TileData[] tiles)
        {

            for (int i = 0; i < tilesOn.Length; i++)
            {
                tilesOn[i].element = this;
                tilesOn[i] = tiles[i];
            }
        }
        public override void UnlinkTile()
        {
            for (int i = 0; i < tilesOn.Length; i++)
            {
                tilesOn[i].element = null;
                tilesOn[i] = null;
            }
        }
    }
    public class Source : Element
    {
        public Source() : base(FlowStrenght._100_, 1) { }
    }
}
