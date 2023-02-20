using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public static class RiverExtension
    {
        private static MapHandler _map;
        public static MapHandler map
        {
            get
            {
                if (_map is null) _map = MapHandler.Instance;
                return _map;
            }
        }

        public static River GenerateRiverCanal(Vector2Int start, Vector2Int end)
        {
            River river = new River(start, end);

            map.GetTile(start).rivers = new List<River>() { river };
            map.GetTile(end).rivers = new List<River>() { river };

            return river;
        }
    }
}
