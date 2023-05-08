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
        public static List<River> linkedRiver = new List<River>();

        public static List<RiverPoint> River2Point(List<Vector2Int> tilesGridPos)
        {
            var result = new List<RiverPoint>();
            //result = tilesGridPos.Select(gridPos => new RiverPoint(level.grid.TileToPos(gridPos), level.tileGrid[gridPos].irrigation))

            var a = riverPalette.FromIrrigation(map.currentFlow[map.GridPos2ID(1, 1)]);
            result = tilesGridPos.Select(
                gridPos => new RiverPoint(
                    grid.TileToPos(gridPos),
                    riverPalette.FromIrrigation(map.currentFlow[map.GridPos2ID(gridPos)]),
                    map.element[map.GridPos2ID(gridPos)] is Lake ? 1 : 0
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
        public static void Extend(this River river, Vector2Int fromPos, Vector2Int toPos)
        {
            if (fromPos != river.endNode
             && fromPos != river.startNode)
            {
                Debug.LogError("Error : try to extend but not from an extremum", river);
                return;
            }
            //Made sure startTile is the endTile
            if (river.endNode != fromPos)
            {
                //Extend river 
                river.UnlinkToGrid();

                var tile = new List<Vector2Int>() { toPos };
                tile.AddRange(river.tiles);
                river.Initialise(tile);

                river.LinkToGrid();
            }
            else
            {
                //Extend river 
                river.UnlinkToGrid();

                var tile = river.tiles;
                tile.Add(toPos);
                river.Initialise(tile);

                river.LinkToGrid();
            }
        }
        public static void Shorten(this River river, Vector2Int eradesTile)
        {
            if (eradesTile != river.endNode && eradesTile != river.startNode)
            {
                Debug.LogError("Error : try to extend but not from an extremum", river);
                return;
            }

            //Extend river 
            river.UnlinkToGrid();

            var tile = river.tiles;
            tile.Remove(eradesTile);
            river.Initialise(tile);

            river.LinkToGrid();
        }
        public static void Merge(this River riverIn, River riverSup)
        {
            if (riverIn.endNode != riverSup.startNode)
            {
                Debug.LogError("Error : try to extend but not valide", riverIn);
                return;
            }
            //Extend river 
            riverIn.UnlinkToGrid();
            riverSup.UnlinkToGrid();
            var tiles = riverIn.tiles.Union(riverSup.tiles).ToList();
            tiles.Distinct();
            riverIn.Initialise(tiles);
            riverIn.LinkToGrid();
        }
        public static (List<Vector2Int>, List<Vector2Int>) Split(this River river, Vector2Int splitPos)
        {
            if (splitPos == river.startNode || splitPos == river.endNode)
            {
                Debug.LogError("Error : try to split a river extremum", river);
                return (null, null);
            }

            int index = river.tiles.IndexOf(splitPos);
            var riverTileA = river.tiles.GetRange(0, index + 1);
            var riverTileB = river.tiles.GetRange(index, river.tiles.Count - index);
            return (riverTileA, riverTileB);
        }
        public static (List<Vector2Int>, List<Vector2Int>) Break(this River river, Vector2Int breakPos)
        {
            if (breakPos == river.startNode || breakPos == river.endNode)
            {
                Debug.LogError("Error : try to break a river extremum", river);
                return (null, null);
            }

            int index = river.tiles.IndexOf(breakPos);
            var riverTileA = river.tiles.GetRange(0, index);
            var riverTileB = river.tiles.GetRange(index + 1, river.tiles.Count - index - 1);
            return (riverTileA, riverTileB);
        }

        public static bool CheckForLoop(this River river, Vector2Int linkTile)
        {
            linkedRiver.Clear();

            int linkTileID = map.GridPos2ID(linkTile);

            var discoveredRiver = map.rivers[linkTileID];//.Where(riv => riv.endNode == linkTile);
            foreach (var riv in discoveredRiver)
            {
                linkedRiver.AddRange(riv.GetParentRiver());
            }

            return linkedRiver.Contains(river);
        }
        public static List<River> GetParentRiver(this River computedRiver, List<River> alreadyCalc = null)
        {
            List<River> result = new List<River>();
            if (alreadyCalc == null)
            {
                alreadyCalc = new List<River>();
            }

            alreadyCalc.Add(computedRiver);

            Vector2Int startTile = computedRiver.startNode;
            int startTileID = map.GridPos2ID(startTile);

            //Select river i don't already check
            var discoveredRiver = map.rivers[startTileID].Where(river => !alreadyCalc.Contains(river)).ToList();
            //Select river giving me water 
            discoveredRiver = discoveredRiver.Where(river => river.endNode == startTile).ToList();

            if (discoveredRiver.Count <= 0) return result;
            result.AddRange(discoveredRiver);

            foreach (var river in discoveredRiver)
            {
                result.AddRange(river.GetParentRiver(alreadyCalc));
            }

            return result;
        }
    }
}