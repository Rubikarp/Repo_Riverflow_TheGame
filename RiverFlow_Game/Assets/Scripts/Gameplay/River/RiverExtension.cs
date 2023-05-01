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
                    riverPalette.FromIrrig(level.tileGrid[gridPos].currentFlow))
                ).ToList();

            return result;
        }
        public static void LinkToGrid(this River river)
        {
            if (river.Lenght < 2)
            {
                Debug.LogError("Error : not enough tiles", river);
                return;
            }

            level.tileGrid[river.tiles[0]].rivers.Add(river);
            level.tileGrid[river.tiles[0]].riverOut.Add(river.tiles[1] - river.tiles[0]);
            for (int i = 1; i < river.tiles.Count-1; i++)
            {
                level.tileGrid[river.tiles[i]].rivers.Add(river);
                level.tileGrid[river.tiles[i]].riverIn.Add(river.tiles[i - 1] - river.tiles[i]);
                level.tileGrid[river.tiles[i]].riverOut.Add(river.tiles[i + 1] - river.tiles[i]);
            }
            level.tileGrid[river.tiles.Last()].rivers.Add(river);
            level.tileGrid[river.tiles.Last()].riverIn.Add(river.tiles.FromEnd(1) - river.tiles.Last());
        }
        public static void UnlinkToGrid(this River river)
        {
            if (river.Lenght < 2)
            {
                Debug.LogError("Error : not enough tiles", river);
                return;
            }

            level.tileGrid[river.tiles[0]].rivers.Remove(river);
            level.tileGrid[river.tiles[0]].riverOut.Remove(river.tiles[1] - river.tiles[0]);
            for (int i = 1; i < river.tiles.Count - 1; i++)
            {
                level.tileGrid[river.tiles[i]].rivers.Remove(river);
                level.tileGrid[river.tiles[i]].riverIn.Remove(river.tiles[i - 1] - river.tiles[i]);
                level.tileGrid[river.tiles[i]].riverOut.Remove(river.tiles[i + 1] - river.tiles[i]);
            }
            level.tileGrid[river.tiles.Last()].rivers.Remove(river);
            level.tileGrid[river.tiles.Last()].riverIn.Remove(river.tiles.FromEnd(1) - river.tiles.Last());
        }
    }
}
