using UnityEditor;
using System.Linq;
using RiverFlow.LD;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace RiverFlow.Core
{
    public class MapHandler : SingletonMonoBehaviour<MapHandler>
    {
        [Header("Component")]
        public WorldGrid grid;
        public DataGrid<TileTopology> topology = InitTopo();
        public MapData mapData;

        public bool showTopo;
        private static DataGrid<TileTopology> InitTopo() => new DataGrid<TileTopology>(new Vector2Int(60, 32));

        [Button] void LoadCurrentMap() => LoadMap(mapData);
        public void LoadMap(MapData mapData)
        {
            this.mapData = mapData;
            topology = new DataGrid<TileTopology>(new Vector2Int(60, 32));
            for (int x = 0; x < topology.Size.x; x++)
            {
                for (int y = 0; y < topology.Size.y; y++)
                {
                    topology.Tiles[x, y].type = mapData.Topology[x, y];
                }
            }
        }
        [Button] void LoadMap()
        {
            topology = new DataGrid<TileTopology>(new Vector2Int(60, 32));
            for (int x = 0; x < topology.Size.x; x++)
            {
                for (int y = 0; y < topology.Size.y; y++)
                {
                    topology.Tiles[x, y].type = mapData.Topology[x, y];
                }
            }
        }

        void Awake()
        {
            if (mapData is null) {/*R*/}
            else LoadMap(mapData);
        }

        protected void OnDrawGizmos()
        {
            if (!showTopo) return;

            Color FromTopo(TileType type)
            {
                switch (type)
                {
                    case TileType.Grass: return Color.green;
                    case TileType.Clay: return Color.red;
                    case TileType.Sand: return Color.yellow;
                    case TileType.Mountain: return Color.grey;
                    default: return Color.magenta;
                }
            }
#if UNITY_EDITOR
            Vector3 startPos = new Vector3(grid.OffSet.x, grid.OffSet.y, 0);
            startPos -= new Vector3(grid.Size.x, grid.Size.y, 0) * 0.5f * grid.cellSize;

            float halfCell = grid.cellSize * 0.5f;

            using (new Handles.DrawingScope())
            {
                for (int x = 0; x < topology.Size.x; x++)
                {
                    for (int y = 0; y < topology.Size.y; y++)
                    {
                        Handles.color = FromTopo(topology.Tiles[x, y].type);
                        Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                    }
                }
            }
#endif
        }
    }

}
