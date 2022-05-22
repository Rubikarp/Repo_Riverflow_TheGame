using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace RiverFlow.Core
{
    public class GameGrid<T>
    {
        [Header("Parameter")]
        public float cellSize = 1;
        public Vector2Int size = new Vector2Int(16, 16);
        public Vector2 offSet = new Vector2Int(0, 0);

        [Header("Data")]
        protected T[] tiles;
        #region Grid-Tile Methodes
        public T GetTile(int x, int y)
        {
            return tiles[x + (y * (size.x))];
        }
        public T GetTile(Vector2Int pos) => GetTile(pos.x, pos.y);
        public void SetTile(int x, int y, T value)
        {
            tiles[x + (y * (size.x))] = value;
        }
        public void SetTile(Vector2Int pos, T value) => SetTile(pos.x, pos.y, value);
        #endregion

        #region Debug
        [BoxGroup("Debug")] protected bool showDebug;
        [BoxGroup("Debug"), ShowIf("showDebug")] protected Color debugLineColor = Color.red;
        [BoxGroup("Debug"), ShowIf("showDebug")] protected bool showCenter;
        [BoxGroup("Debug"),ShowIf("showCenter")] protected Color debugCenterColor = Color.black;
        #endregion

        #region Grid<->World Convertion
        /// <summary>
        /// Convert a GameGrid Pos to a worldPos
        /// WARNING ! The result can be extrapolate farther than the GridSize
        /// </summary>
        /// <param name="posInGrid"> Position on the GameGrid (can be negative)</param>
        /// <returns></returns>
        public Vector3 TileToPos(Vector2Int posInGrid)
        {
            //Get bottom left corner |_
            Vector3 bottomLeft = new Vector3(offSet.x, offSet.y, 0);
            bottomLeft -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;
            //Centre de la case
            bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

            Vector3 result = bottomLeft + (new Vector3(posInGrid.x, posInGrid.y, 0) * cellSize);

            return result;
        }
        public Vector3 TileToPos(int x, int y) 
        { 
            return TileToPos(new Vector2Int(x,y)); 
        }
        /// <summary>
        /// Convert a Point on the GamePlane to the GameGrid Pos related
        /// WARNING ! it can result a pos outside of the actual grid
        /// </summary>
        /// <param name="planePos"> point on the plane where you look for the tile</param>
        /// <returns></returns>
        public Vector2Int PosToTile(Vector3 planePos)
        {
            //Get bottom left corner |_
            Vector3 bottomLeft = new Vector3(offSet.x, offSet.y, 0);
            bottomLeft -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;
            //Pas de centrage sur la case car floor, si round activer la ligne
            //bottomLeft += new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

            Vector3 posRelaToGrid = planePos - bottomLeft;
            float returnToCellsize = 1 / cellSize;
            Vector2Int result = new Vector2Int(Mathf.FloorToInt(posRelaToGrid.x * returnToCellsize), Mathf.FloorToInt(posRelaToGrid.y * returnToCellsize));

            return result;
        }
        #endregion

        protected void DefaultGizmoDraw()
        {
            if (showDebug)
            {
                Vector3 startPos = new Vector3(offSet.x, offSet.y, 0);
                startPos -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;

                float halfCell = cellSize * 0.5f;

                if (showCenter)
                {
                    #region center point
                    Color temp = Gizmos.color;
                    Gizmos.color = debugCenterColor;
                    for (int x = 0; x < size.x; x++)
                    {
                        for (int y = 0; y < size.y; y++)
                        {
                            Gizmos.DrawWireSphere(startPos + new Vector3(x * cellSize, y * cellSize, 0) + new Vector3(halfCell, halfCell, 0), halfCell * 0.8f);
                        }
                    }
                    Gizmos.color = temp;
                    #endregion
                }

                //GameGrid decals
                for (int x = 0; x <= size.x; x++)
                {
                    Debug.DrawRay(startPos + new Vector3(cellSize * x, 0, 0), Vector3.up * size.y * cellSize, debugLineColor);
                }
                for (int y = 0; y <= size.y; y++)
                {
                    Debug.DrawRay(startPos + new Vector3(0, cellSize * y, 0), Vector3.right * size.x * cellSize, debugLineColor);
                }
            }
        }
    }
}
