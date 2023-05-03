using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    //[System.Serializable]
    public class TileMap : MonoBehaviour
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

        [Header("Info")]
        public Vector2Int size;

        [Header("State")]
        public Topology[] topology;

        [Header("State")]
        public FlowStrenght[] irrigation;
        public FlowStrenght[] currentFlow;
        public FlowStrenght[] previousFlow;

        [Header("State")]
        public Plant[] plant;
        public Element[] element;
        public List<Vector2Int>[] riverIn;
        public List<Vector2Int>[] riverOut;
        public List<River>[] rivers;

        private int GridPos2ID(int x, int y) => x + y * (size.x - 1);
        private int GridPos2ID(Vector2Int pos) => pos.x + pos.y * (size.x - 1);

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
            int id;

            //Record currentFlow
            previousFlow = currentFlow;

            //Compute new Flow
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                {
                    id = GridPos2ID(x, y);
                    flow = FlowStrenght._00_;
                    gridPos = new Vector2Int(x, y);

                    for (int i = 0; i < riverIn[id].Count; i++)
                    {
                        lookPos = gridPos + riverIn[id][i];
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= size.x || lookPos.y >= size.y) continue;
                        flow += (int)previousFlow[GridPos2ID(lookPos)];
                    }

                    if (element[id] != null)
                    {
                        flow += (int)element[id].irrigationLvl;
                    }

                    currentFlow[id] = (FlowStrenght)Mathf.Clamp((int)flow, 0, 4);
                }
        }
        public void ComputeIrig()
        {
            Vector2Int gridPos;
            FlowStrenght flow;
            int id;

            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                {
                    id = GridPos2ID(x, y);
                    flow = FlowStrenght._00_;
                    gridPos = new Vector2Int(x, y);

                    if (currentFlow[id] == FlowStrenght._100_)
                    {
                        irrigation[id] = FlowStrenght._100_;
                        continue;
                    }
                    foreach (var lookPos in lookPosDist2)
                    {
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= size.x || lookPos.y >= size.y) continue;
                        if (element[id] is Lake)
                        {
                            irrigation[GridPos2ID(gridPos + lookPos)] = FlowStrenght._100_;
                            continue;
                        }
                    }
                    foreach (var lookPos in lookPosDist1)
                    {
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= size.x || lookPos.y >= size.y) continue;
                        flow += (int)currentFlow[GridPos2ID(gridPos + lookPos)];
                    }

                    irrigation[id] = (FlowStrenght)Mathf.Clamp((int)flow, 0, 4);
                }
        }
    }
}
