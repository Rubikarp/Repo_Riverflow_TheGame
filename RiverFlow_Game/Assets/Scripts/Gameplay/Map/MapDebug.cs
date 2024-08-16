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
    public class MapDebug : MonoBehaviour
    {
        [SerializeField] TopologyPalette topologyPalette;
        [SerializeField] RiverPalette riverPalette;
        [SerializeField] bool showTopo;
        [SerializeField] bool showIrrigation;

        [Header("Component")]
        [SerializeField] WorldGrid grid;
        [SerializeField] TileMap tileMap;


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
                    for (int x = 0; x < tileMap.Size.x; x++)
                    {
                        for (int y = 0; y < tileMap.Size.y; y++)
                        {
                            Handles.color = topologyPalette.FromTopo(tileMap.GetTopology(new Vector2Int(x, y)));
                            Extension_Handles.DrawWireSquare(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * grid.cellSize * 0.75f);
                        }
                    }
                }
                if (showIrrigation)
                {
                    for (int x = 0; x < tileMap.Size.x; x++)
                    {
                        for (int y = 0; y < tileMap.Size.y; y++)
                        {
                            Handles.color = riverPalette.FromIrrigation(tileMap.currentFlow[tileMap.GridPos2ID(x, y)]);
                            Handles.DrawWireDisc(startPos + new Vector3(x * grid.cellSize, y * grid.cellSize, 0) + new Vector3(halfCell, halfCell, 0), Vector3.back, grid.cellSize * 0.25f);
                        }
                    }
                }
            }
        }
#endif
    }

}
