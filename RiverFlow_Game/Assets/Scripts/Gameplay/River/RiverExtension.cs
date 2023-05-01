using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiverFlow.Core
{
    public static class RiverExtension
    {
        private static LevelHandler _level;
        public static LevelHandler level
        {
            get
            {
                if (_level is null) _level = LevelHandler.Instance;
                return _level;
            }
        }
        private static RiverPalette _riverPalette;
        public static RiverPalette riverPalette
        {
            get
            {
                if (_riverPalette is null) _riverPalette = RiverPalette.Instance;
                return _riverPalette;
            }
        }

        public static List<RiverPoint> River2Point(List<Vector2Int> tilesGridPos)
        {
            var result = new List<RiverPoint>();
            //result = tilesGridPos.Select(gridPos => new RiverPoint(level.grid.TileToPos(gridPos), level.tileGrid[gridPos].irrigation))
            
            result = tilesGridPos.Select(
                gridPos => new RiverPoint(
                    level.grid.TileToPos(gridPos), 
                    Color.red)
                ).ToList();

            return result;
        }
    }
}
