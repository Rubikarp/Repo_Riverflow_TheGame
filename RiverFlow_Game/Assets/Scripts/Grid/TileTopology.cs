using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public class TileTopology : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int GridPos { get; private set; }

        [Header("State")]
        public TileType type;
        public FlowStrenght riverStrenght;

        void Start()
        {
        
        }

        void Update()
        {
        
        }
    }
}
