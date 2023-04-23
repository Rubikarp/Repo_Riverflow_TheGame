using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow.Core
{

    public abstract class Element : TileObject
    {
        [Header("Element Value")]
        public readonly FlowStrenght irrigationLvl = FlowStrenght._100_;

        public Element(FlowStrenght irrigationStrenght, Vector2Int pos) :base(pos)
        {
            irrigationLvl = irrigationStrenght;
        }
    }
}
