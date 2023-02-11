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
    public class LevelHandler : SingletonMonoBehaviour<LevelHandler>
    {
        [Header("Component")]
        public WorldGrid grid;
        public DataGrid<TileData> topology = InitTopo();
        public MapData mapData;

        public bool showTopo;
        private static DataGrid<TileData> InitTopo() => new DataGrid<TileData>(new Vector2Int(60, 32));
        
        private void Awake()
        {
            if (mapData is null) {/*R*/}
            else LoadMap(mapData);
        }

        [Button] void LoadCurrentMap() => LoadMap(mapData);
        public void LoadMap(MapData mapData)
        {
            this.mapData = mapData;
            topology = new DataGrid<TileData>(new Vector2Int(60, 32));
            for (int x = 0; x < topology.Size.x; x++)
            {
                for (int y = 0; y < topology.Size.y; y++)
                {
                    topology.Tiles[x, y].topology = mapData.Topology[x, y];
                }
            }
        }


#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            if (!showTopo) return;
            Color FromTopo(TileTopology topo)
            {
                switch (topo)
                {
                    case TileTopology.Grass: return Color.green;
                    case TileTopology.Clay: return Color.red;
                    case TileTopology.Sand: return Color.yellow;
                    case TileTopology.Mountain: return Color.grey;
                    case TileTopology.None: return Color.magenta;
                    default: return Color.magenta;
                }
            }
            Vector3 startPos = new Vector3(grid.OffSet.x, grid.OffSet.y, 0);
            startPos -= new Vector3(grid.Size.x, grid.Size.y, 0) * 0.5f * grid.cellSize;

            float halfCell = grid.cellSize * 0.5f;

            using (new Handles.DrawingScope())
            {
                for (int x = 0; x < topology.Size.x; x++)
                {
                    for (int y = 0; y < topology.Size.y; y++)
                    {
                        Handles.color = FromTopo(topology.Tiles[x, y].topology);
                        Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                    }
                }
            }
        }
#endif
    }

}
