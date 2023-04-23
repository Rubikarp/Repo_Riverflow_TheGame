using UnityEngine;

namespace RiverFlow.Core
{
    public class Lake : Element
    {
        public Vector2Int[] pattern = new Vector2Int[3] { Vector2Int.left, Vector2Int.zero, Vector2Int.right};

        public Lake(Vector2Int pos) : base(FlowStrenght._100_, pos) 
        {

        }
    }
}
