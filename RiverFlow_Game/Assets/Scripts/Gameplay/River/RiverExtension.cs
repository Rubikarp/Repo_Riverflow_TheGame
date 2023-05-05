using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiverFlow.Core
{
    public static class RiverExtension
    {
        private static TileMap _map;
        private static WorldGrid _grid;
        private static RiverPalette _riverPalette;
        public static TileMap map
        {
            get
            {
                if (_map is null) _map = TileMap.Instance;
                return _map;
            }
        }
        public static WorldGrid grid
        {
            get
            {
                if (_grid is null) _grid = WorldGrid.Instance;
                return _grid;
            }
        }
        public static RiverPalette riverPalette
        {
            get
            {
                if (_riverPalette is null) _riverPalette = Resources.Load("RiverPalette") as RiverPalette;
                return _riverPalette;
            }
        }

        public static List<RiverPoint> River2Point(List<Vector2Int> tilesGridPos)
        {
            var result = new List<RiverPoint>();
            //result = tilesGridPos.Select(gridPos => new RiverPoint(level.grid.TileToPos(gridPos), level.tileGrid[gridPos].irrigation))

            var a = riverPalette.FromIrrigation(map.currentFlow[map.GridPos2ID(1,1)]);
            result = tilesGridPos.Select(
                gridPos => new RiverPoint(
                    grid.TileToPos(gridPos),
                    riverPalette.FromIrrigation(map.currentFlow[map.GridPos2ID(gridPos)]),
                    map.element[map.GridPos2ID(gridPos)] is Lake ? 1 :0
                )).ToList();

            return result;
        }

        public static void LinkToGrid(this River river)
        {
            if (river.Lenght < 2)
            {
                Debug.LogError("Error : not enough tiles", river);
                return;
            }

            int id = _map.GridPos2ID(river.tiles[0]);
            _map.rivers[id].Add(river);
            _map.riverOut[id].Add(river.tiles[1] - river.tiles[0]);

            for (int i = 1; i < river.tiles.Count - 1; i++)
            {
                id = _map.GridPos2ID(river.tiles[i]);
                _map.rivers[id].Add(river);
                _map.riverIn[id].Add(river.tiles[i - 1] - river.tiles[i]);
                _map.riverOut[id].Add(river.tiles[i + 1] - river.tiles[i]);
            }

            id = _map.GridPos2ID(river.tiles.Last());
            _map.rivers[id].Add(river);
            _map.riverIn[id].Add(river.tiles.FromEnd(1) - river.tiles.Last());
        }
        public static void UnlinkToGrid(this River river)
        {
            if (river.Lenght < 2)
            {
                Debug.LogError("Error : not enough tiles", river);
                return;
            }

            int id = _map.GridPos2ID(river.tiles[0]);
            _map.rivers[id].Remove(river);
            _map.riverOut[id].Remove(river.tiles[1] - river.tiles[0]);

            for (int i = 1; i < river.tiles.Count - 1; i++)
            {
                id = _map.GridPos2ID(river.tiles[i]);
                _map.rivers[id].Remove(river);
                _map.riverIn[id].Remove(river.tiles[i - 1] - river.tiles[i]);
                _map.riverOut[id].Remove(river.tiles[i + 1] - river.tiles[i]);
            }

            id = _map.GridPos2ID(river.tiles.Last());
            _map.rivers[id].Remove(river);
            _map.riverIn[id].Remove(river.tiles.FromEnd(1) - river.tiles.Last());
        }

        public static void Reverse(this River river)
        {
            river.UnlinkToGrid();
            river.tiles.Reverse();
            river.LinkToGrid();
        }
        public static void Extend(this River river, Vector2Int newPos)
        {
            if (river.startNode != newPos || river.endNode != newPos)
            {
                Debug.LogError("Error : try to extend but not from an extremum", river);
                return;
            }
            //Made sure startTile is the endTile
            if (river.startNode != newPos)
            {
                river.Reverse();
            }
            //Extend river 
            river.UnlinkToGrid();
            river.Extend(river.tiles, newPos);
            river.LinkToGrid();
        }


    }
}
