using System.Linq;
using UnityEngine;
using RiverFlow.LD;
using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.Core
{
    public class LevelHandler : SingletonMonoBehaviour<LevelHandler>
    {
        public bool showTopo;

        [Header("Component")]
        public WorldGrid grid;
        [HideInInspector] public TileGrid tileGrid = new TileGrid(new Vector2Int(60, 32));


        public void LoadMap(MapData mapData)
        {
            tileGrid = new TileGrid(mapData.Size);
            for (int x = 0; x < tileGrid.Size.x; x++)
            {
                for (int y = 0; y < tileGrid.Size.y; y++)
                {
                    tileGrid.GetTile(x, y).topology = mapData.GetTopology(x, y);
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
                for (int x = 0; x < tileGrid.Size.x; x++)
                {
                    for (int y = 0; y < tileGrid.Size.y; y++)
                    {
                        Handles.color = FromTopo(tileGrid.GetTile(x, y).topology);
                        Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                    }
                }
            }
        }
#endif
    }

}
