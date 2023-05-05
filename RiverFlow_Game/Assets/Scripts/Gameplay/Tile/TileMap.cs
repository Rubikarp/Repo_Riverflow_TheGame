using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using RiverFlow.LD;

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
        [SerializeField, ReadOnly] private Vector2Int size;
        public Vector2Int Size { get => size; }

        [Header("State")]
        [HideInInspector] public Topology[] topology;

        [Header("State")]
        [HideInInspector] public FlowStrenght[] currentFlow;
        [HideInInspector] private FlowStrenght[] previousFlow;
        [Space(10)]
        [HideInInspector] public FlowStrenght[] irrigation;
        [Space(10)]
        [HideInInspector] public FlowStrenght[] extraFlow;
        [HideInInspector] public FlowStrenght[] extraIrrig;

        [Header("Linked Element")]
        [HideInInspector] public Plant[] plant;
        [HideInInspector] public Element[] element;

        [Header("River")]
        [HideInInspector] public List<River>[] rivers;
        [HideInInspector] public List<Vector2Int>[] riverIn;
        [HideInInspector] public List<Vector2Int>[] riverOut;

        [Header("Debug")]
        [SerializeField] private Vector2Int lookPos;
        [SerializeField, ReadOnly] private TileData lookedTile;
        private void OnValidate()
        {
            lookPos = new Vector2Int(
                Mathf.Clamp(lookPos.x, 0, size.x - 1),
                Mathf.Clamp(lookPos.y, 0, size.y - 1));
            UpdateDebugLookedTile();
        }
        [Button]
        private void UpdateDebugLookedTile()
        {
            int id = GridPos2ID(lookPos);
            lookedTile.gridPos = lookPos;
            lookedTile.topology = topology[id];
            lookedTile.irrigation = irrigation[id];
            lookedTile.currentFlow = currentFlow[id];
            lookedTile.plant = plant[id];
            lookedTile.element = element[id];
            lookedTile.riverIn = riverIn[id];
            lookedTile.riverOut = riverOut[id];
            lookedTile.rivers = rivers[id];
        }

        private int GridPos2ID(int x, int y) => x + y * (size.x - 1);
        private int GridPos2ID(Vector2Int pos) => pos.x + pos.y * (size.x - 1);

        public TileMap()
        {
            Initialize(new Vector2Int(64, 32));
        }
        public void Initialize(Vector2Int size)
        {
            this.size = size;
            topology = new Topology[size.x * size.y];

            currentFlow = new FlowStrenght[size.x * size.y];
            previousFlow = new FlowStrenght[size.x * size.y];

            irrigation = new FlowStrenght[size.x * size.y];

            extraFlow = new FlowStrenght[size.x * size.y];
            extraIrrig = new FlowStrenght[size.x * size.y];

            plant = new Plant[size.x * size.y];
            element = new Element[size.x * size.y];

            rivers = new List<River>[size.x * size.y];
            riverIn = new List<Vector2Int>[size.x * size.y];
            riverOut = new List<Vector2Int>[size.x * size.y];
        }
        public void LoadMap(MapData map)
        {
            this.size = map.size;
            topology = map.topology;

            currentFlow = new FlowStrenght[size.x * size.y];
            previousFlow = new FlowStrenght[size.x * size.y];

            irrigation = new FlowStrenght[size.x * size.y];

            extraFlow = new FlowStrenght[size.x * size.y];
            extraIrrig = new FlowStrenght[size.x * size.y];

            plant = new Plant[size.x * size.y];
            element = new Element[size.x * size.y];

            rivers = new List<River>[size.x * size.y];
            riverIn = new List<Vector2Int>[size.x * size.y];
            riverOut = new List<Vector2Int>[size.x * size.y];
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
                    flow += (int)extraFlow[id];

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
                    flow = irrigation[id];
                    gridPos = new Vector2Int(x, y);

                    foreach (var lookPos in lookPosDist1)
                    {
                        if (lookPos.x < 0 || lookPos.y < 0 || lookPos.x >= size.x || lookPos.y >= size.y) continue;
                        flow = (FlowStrenght)Mathf.Max((int)flow, (int)currentFlow[GridPos2ID(gridPos + lookPos)]);
                    }
                    flow += (int)extraIrrig[id];

                    irrigation[id] = (FlowStrenght)Mathf.Clamp((int)flow, 0, 4);
                }
        }
    }
}
