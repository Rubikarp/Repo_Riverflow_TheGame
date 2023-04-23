using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.Jobs;
using Unity.Collections;
using System.Collections;

namespace RiverFlow.Core
{
    [System.Serializable]
    public class TileGrid
    {
        [SerializeField] private Vector2Int size;
        public Vector2Int Size { get => size; private set => size = value; }

        [HideInInspector] private TileData[] tiles;
        // Define an indexer (https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/?redirectedfrom=MSDN)
        public TileData this[Vector2Int pos] { get { return this[pos.x, pos.y]; } }
        public TileData this[int x, int y] { get { return tiles[x + y * size.x]; } }


        #region Topology
        public void SetTopo(Topology topo, Vector2Int pos) => SetTopo(topo, pos.x, pos.y);
        public void SetTopo(Topology topo, int x, int y) => tiles[x + (y * size.x)].topology = topo;
        #endregion Topology

        #region Element
        public void UnlinkElement(Vector2Int pos) => UnlinkElement(pos.x, pos.y);
        public void UnlinkElement(int x, int y) => tiles[x + (y * size.x)].element = null;
        public void LinkElement(Vector2Int pos, Element element) => LinkElement(pos.x, pos.y, element);
        public void LinkElement(int x, int y, Element element) => tiles[x + (y * size.x)].element = element;
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
            {
                for (int x = 0; x < Size.x; x++)
                {
                    tiles[x + (y * size.x)] = new TileData(x, y);
                }
            }
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
