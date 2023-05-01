using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.Jobs;
using Unity.Collections;
using System.Collections;
using System.Linq;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileGrid
    {
        [SerializeField] private Vector2Int size;
        public Vector2Int Size { get => size; private set => size = value; }

        [HideInInspector] private TileData[] tiles;
        [HideInInspector] public TileData[] Tiles { get => tiles; }
        // Define an indexer (https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/?redirectedfrom=MSDN)
        public TileData this[Vector2Int pos] { get { return this[pos.x, pos.y]; } }
        public TileData this[int x, int y] { get 
            {
                //if (x < 0 || y < 0 || x >= Size.x || y >= Size.y)Debug.LogError(x + " / " + y);
                //if (x + y * (size.x - 1) >= tiles.Length) Debug.LogError(x + " / " + y + " = " + (x + y * (size.x - 1)) + " on " + tiles.Length);
                return tiles[x + y * (size.x-1)]; 
            } 
        }

        #region Tiles
        public void SetTile(TileData tile, Vector2Int pos) => SetTile(tile, pos.x, pos.y);
        public void SetTile(TileData tile, int x, int y) => tiles[x + (y * (size.x - 1))] = tile;
        #endregion Tiles
        #region Topology
        public void SetTopo(Topology topo, Vector2Int pos) => SetTopo(topo, pos.x, pos.y);
        public void SetTopo(Topology topo, int x, int y) => tiles[x + (y * (size.x - 1))].topology = topo;
        #endregion Topology
        #region Irrigation
        public void SetIrrigation(FlowStrenght flow, Vector2Int pos) => SetIrrigation(flow, pos.x, pos.y);
        public void SetIrrigation(FlowStrenght flow, int x, int y) => tiles[x + (y * (size.x - 1))].irrigation = flow;
        #endregion Irrigation
        #region Flow
        public void SetCurrentFlow(FlowStrenght flow, Vector2Int pos) => SetCurrentFlow(flow, pos.x, pos.y);
        public void SetCurrentFlow(FlowStrenght flow, int x, int y) => tiles[x + (y * (size.x - 1))].currentFlow = flow;
        public void RegisterPreviousFlow(Vector2Int pos) => RegisterPreviousFlow(pos.x, pos.y);
        public void RegisterPreviousFlow(int x, int y) => tiles[x + (y * (size.x - 1))].previousFlow = tiles[x + (y * (size.x - 1))].currentFlow;
        #endregion Flow
        #region Element
        public void UnlinkElement(Vector2Int pos) => UnlinkElement(pos.x, pos.y);
        public void UnlinkElement(int x, int y) => tiles[x + (y * (size.x - 1))].element = null;
        public void LinkElement(Vector2Int pos, Element element) => LinkElement(pos.x, pos.y, element);
        public void LinkElement(int x, int y, Element element) => tiles[x + (y * (size.x - 1))].element = element;
        #endregion Element

        #region Plante
        public void UnlinkPlant(Vector2Int pos) => UnlinkElement(pos.x, pos.y);
        public void UnlinkPlant(int x, int y) => tiles[x + (y * size.x)].plant = null;
        public void LinkPlant(Vector2Int pos, Plant plant) => LinkPlant(pos.x, pos.y, plant);
        public void LinkPlant(int x, int y, Plant plant) => tiles[x + (y * size.x)].plant = plant;
        #endregion Plante

        public TileGrid(Vector2Int size)
        {
            Size = size;
            tiles = new TileData[Size.x * Size.y];
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    SetTile(new TileData(x, y),x,y);
        }

        struct FlowUpdateJob : IJobParallelFor
        {
            [ReadOnly] public Vector2Int size;
            public NativeArray<TileData> tiles;
            public int GetID(int x, int y) { return x + (y * size.x); }

            public void Execute(int i)
            {
                TileData tile = tiles[i];
                Vector2Int pos, offset;

                tile.previousFlow = tiles[i].currentFlow;
                tile.currentFlow = FlowStrenght._00_;

                for (int j = 0; j < tiles[i].riverIn.Count; j++)
                {
                    offset = tiles[i].riverIn[j];
                    pos = tile.gridPos + offset;
                    tile.currentFlow += (int)tiles[pos.x + (pos.y * size.x)].currentFlow;
                }
                if (!(tiles[i].element is null))
                {
                    tile.currentFlow += (int)tiles[i].element.irrigationLvl;
                }

                tile.currentFlow = (FlowStrenght)Mathf.Clamp((int)tiles[i].currentFlow, 0, 4);
            }
        }

    }

}
