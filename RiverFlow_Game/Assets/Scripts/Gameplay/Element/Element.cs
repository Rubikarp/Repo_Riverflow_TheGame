using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow.Core
{

    public abstract class Element : TileObject
    {
        [Header("Element Value")]
        public readonly FlowStrenght irrigationLvl;

        public Element(FlowStrenght strenght = FlowStrenght._100_, int tileSize = 1):base(tileSize)
        {
            irrigationLvl = strenght;
        }

        public override void LinkToTile(TileData tile)
        {
            tilesOn[0] = tile;
            tilesOn[0].element = this;
        }
        public override void UnlinkTile()
        {
            tilesOn[0] = null;
            tilesOn[0].element = null;
        }
    }
}
