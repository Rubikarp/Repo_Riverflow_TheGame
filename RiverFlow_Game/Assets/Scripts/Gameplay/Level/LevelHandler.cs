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
        static readonly Vector2Int[] lookPosDist1 = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1,-1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        };
        static readonly Vector2Int[] lookPosDist2 = new Vector2Int[] {
            new Vector2Int( 0, 2),
            new Vector2Int( 1, 2),
            new Vector2Int( 2, 2),
            new Vector2Int( 2, 1),
            new Vector2Int( 2, 0),
            new Vector2Int( 2,-1),
            new Vector2Int( 2,-2),
            new Vector2Int( 1,-2),
            new Vector2Int( 0,-2),
            new Vector2Int(-1,-2),
            new Vector2Int(-2,-2),
            new Vector2Int(-2,-1),
            new Vector2Int(-2, 0),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, 2),
            new Vector2Int(-1, 2),
        };


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

        private void Update()
        {
            WaterStep();
        }

        [Button]
        public void WaterStep()
        {
            ComputeFlow();
            ComputeIrig();

        }
        public void ComputeFlow()
        {
            Vector2Int gridPos;
            Vector2Int lookPos;
            FlowStrenght flow;

            //Record currentFlow
            for (int x = 0; x < tileGrid.Size.x; x++)
                for (int y = 0; y < tileGrid.Size.y; y++)
                {
                    gridPos = tileGrid[x, y].gridPos;
                    tileGrid.RegisterPreviousFlow(gridPos);
                }
            //Compute new Flow
            for (int x = 0; x < tileGrid.Size.x; x++)
                for (int y = 0; y < tileGrid.Size.y; y++)
                {
                    flow = FlowStrenght._00_;
                    gridPos = new Vector2Int(x, y);

                    for (int i = 0; i < tileGrid[gridPos].riverIn.Count; i++)
                    {
                        lookPos = gridPos + tileGrid[gridPos].riverIn[i];
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= tileGrid.Size.x || lookPos.y >= tileGrid.Size.y) continue;
                        flow += (int)tileGrid[lookPos].previousFlow;
                    }

                    if (tileGrid[gridPos].element != null)
                    {
                        flow += (int)tileGrid[gridPos].element.irrigationLvl;
                    }

                    flow = (FlowStrenght)Mathf.Clamp((int)flow, 0, 4);
                    tileGrid.SetCurrentFlow(flow, gridPos);
                }
        }
        public void ComputeIrig()
        {
            Vector2Int gridPos;
            FlowStrenght flow;

            for (int x = 0; x < tileGrid.Size.x; x++)
                for (int y = 0; y < tileGrid.Size.y; y++)
                {
                    flow = FlowStrenght._00_;
                    gridPos = new Vector2Int(x, y);

                    if(tileGrid[gridPos].currentFlow == FlowStrenght._100_)
                    {
                        tileGrid.SetCurrentFlow(FlowStrenght._100_, gridPos);
                        continue;
                    }
                    foreach (var lookPos in lookPosDist2)
                    {
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= tileGrid.Size.x || lookPos.y >= tileGrid.Size.y) continue;
                        if (tileGrid[lookPos].element is Lake)
                        {
                            tileGrid.SetCurrentFlow(FlowStrenght._100_, gridPos);
                            continue;
                        }
                    }
                    foreach (var lookPos in lookPosDist1)
                    {
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= tileGrid.Size.x || lookPos.y >= tileGrid.Size.y) continue;
                        flow += (int)tileGrid[lookPos].currentFlow;
                    }

                    flow = (FlowStrenght)Mathf.Clamp((int)flow, 0, 4);
                    tileGrid.SetCurrentFlow(flow, gridPos);
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
                    var palette = Resources.Load("TopologyPalette") as TopologyPalette;
                    for (int x = 0; x < tileGrid.Size.x; x++)
                    {
                        for (int y = 0; y < tileGrid.Size.y; y++)
                        {
                            Handles.color = palette.FromTopo(tileGrid[x, y].topology);
                            Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                        }
                    }
                }
                if (showIrrigation)
                {
                    var palette = Resources.Load("RiverPalette") as RiverPalette;
                    for (int x = 0; x < tileGrid.Size.x; x++)
                    {
                        for (int y = 0; y < tileGrid.Size.y; y++)
                        {
                            Handles.color = palette.FromIrrigation(tileGrid[x, y].currentFlow);
                            Handles.DrawWireDisc(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), Vector3.back, grid.cellSize * 0.25f);
                        }
                    }
                }
            }
        }
#endif
    }

}
