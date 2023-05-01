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
        public bool showIrrigation;

        [Header("Component")]
        public WorldGrid grid;
        public TileGrid tileGrid;

        public void LoadMap(MapData mapData)
        {
            tileGrid = new TileGrid(mapData.Size);
            for (int x = 0; x < tileGrid.Size.x; x++)
            {
                for (int y = 0; y < tileGrid.Size.y; y++)
                {
                    tileGrid.SetTopo(mapData[x, y], x, y);
                }
            }
        }

        public void WaterStep()
        {
            ComputeFlow();
            ComputeIrig();

        }
        public void ComputeFlow()
        {

        }
        public void ComputeIrig()
        {

        }

        Color FromTopo(Topology topo)
        {
            switch (topo)
            {
                case Topology.Grass: return Color.green;
                case Topology.Clay: return Color.red;
                case Topology.Sand: return Color.yellow;
                case Topology.Mountain: return Color.grey;
                case Topology.None: return Color.magenta;
                default: return Color.magenta;
            }
        }
        Color FromIrrigation(FlowStrenght flow)
        {
            switch (flow)
            {
                case FlowStrenght._00_: return Color.black;
                case FlowStrenght._25_: return Color.grey;
                case FlowStrenght._50_: return Color.cyan;
                case FlowStrenght._75_: return Color.blue;
                case FlowStrenght._100_: return Color.white;
                default: return Color.magenta;
            }
        }

#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            Vector3 startPos = new Vector3(grid.OffSet.x, grid.OffSet.y, 0);
            startPos -= new Vector3(grid.Size.x, grid.Size.y, 0) * 0.5f * grid.cellSize;

            float halfCell = grid.cellSize * 0.5f;

            using (new Handles.DrawingScope())
            {
                if (showTopo)
                {
                    for (int x = 0; x < tileGrid.Size.x; x++)
                    {
                        for (int y = 0; y < tileGrid.Size.y; y++)
                        {
                            Handles.color = FromTopo(tileGrid[x, y].topology);
                            Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                        }
                    }
                }
                if (showIrrigation)
                {
                    for (int x = 0; x < tileGrid.Size.x; x++)
                    {
                        for (int y = 0; y < tileGrid.Size.y; y++)
                        {
                            Handles.color = FromIrrigation(tileGrid[x, y].currentFlow);
                            Handles.DrawWireDisc(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), Vector3.back, grid.cellSize * 0.25f);
                        }
                    }
                }
            }
        }
#endif
    }

}
