using UnityEngine;
using NaughtyAttributes;
using RiverFlow.Gameplay.Interaction;
using Unity.Mathematics;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.Core
{
    public class WorldGrid : SingletonMonoBehaviour<WorldGrid>
    {
        [Header("Parameter")]
        public float cellSize = 1;
        public float halfCellSize => cellSize * 0.5f;
        [OnValueChanged("UpdatePlane"), SerializeField] Vector2Int size = new Vector2Int(60, 32);
        [OnValueChanged("UpdatePlane"), SerializeField] Vector2 offSet = new Vector2Int(0, 0);
        public Vector2Int Size { get => size; }
        public Vector2 OffSet { get => offSet; }

        [SerializeField] InteractionPlane interactPlane;

        public void SetLimit(Vector2Int size, Vector2 offSet = new Vector2())
        {
            this.size = size;
            this.offSet = offSet;
            UpdatePlane();
        }
        private void UpdatePlane() => interactPlane.SetLimit(size, offSet);

        [Foldout("Debug"), SerializeField] protected bool showGrid;
        [Foldout("Debug"), SerializeField] protected Color debugLineColor = Color.blue;
        [Foldout("Debug"), SerializeField] protected bool showCenter;
        [Foldout("Debug"), SerializeField] protected Color debugCenterColor = Color.green;


        #region Grid<->World Convertion
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
            bottomLeft -= new Vector3(size.x, size.y, 0) * halfCellSize;

            planePos -= bottomLeft;

            planePos += new Vector3(halfCellSize, halfCellSize, 0);

            Vector2Int risult = Vector2Int.zero;
            //Reference : https://www.shadertoy.com/view/fdtGDH
            //void Hexagone_float(float2 UV, out float2 hexaUV, out float2 hexaIndex)
            //{
            //    //sqrt(3) = 1.73205080757
            //    const float2 s = float2(1.7320508, 1);
            //
            //    float4 hC = floor(float4(UV, UV - float2(1, .5)) / s.xyxy) + .5;
            //    // Centering the coordinates with the hexagon centers above.
            //    float4 h = float4(UV - hC.xy * s, UV - (hC.zw + .5) * s);
            //
            //
            //    float4 result = dot(h.xy, h.xy) < dot(h.zw, h.zw)
            //                    ? float4(h.xy, hC.xy)
            //                    : float4(h.zw, hC.zw + .5);
            //
            //    hexaUV = result.xy;
            //    hexaIndex = result.zw;
            //}

            //Get result.zw for the index
            Vector2 scale = cellSize  * new Vector2(Mathf.Sqrt(3), 3/2f);
            risult = Vector2Int.FloorToInt(planePos / scale);
            //Get result.xy for the UV


            //Hex chip offset
            //risult.x = Mathf.FloorToInt(planePos.x / (cellSize * Mathf.Sqrt(3)));
            //risult.y = Mathf.FloorToInt(planePos.y / (cellSize * 1.5f));

            return risult;
        }
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
            bottomLeft -= new Vector3(size.x, size.y, 0) * halfCellSize;

            Vector3 risult = Vector3.zero;
            //Hex chip offset
            //var x = size * sqrt(3) * (hex.col + 0.5 * (hex.row & 1))
            //var y = size * 3 / 2 * hex.row
            risult.x = cellSize * Mathf.Sqrt(3) * (posInGrid.x + 0.5f * (posInGrid.y % 2));
            risult.y = cellSize * 1.5f * posInGrid.y;

            risult += bottomLeft;
            return risult;
        }
        public Vector3 TileToPos(int x, int y) => TileToPos(new Vector2Int(x, y));
        #endregion

        protected void OnDrawGizmos()
        {
            Vector3 startPos = new Vector3(offSet.x, offSet.y, 0);
            startPos -= new Vector3(size.x, size.y, 0) * 0.5f * cellSize;

            float halfCell = cellSize * 0.5f;

#if UNITY_EDITOR
            if (showGrid)
            {
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
            
            using (new Handles.DrawingScope())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int tile = PosToTile(mousePos);
                Vector3 pos = TileToPos(tile);
                Handles.color = Color.red;
                Handles.DrawWireDisc(pos, Vector3.back, cellSize * .75f);

                if (showCenter)
                {
                    for (int x = 0; x < size.x; x++)
                    {
                        for (int y = 0; y < size.y; y++)
                        {
                            Handles.color = debugCenterColor;
                            var originPos = startPos;
                            originPos += Vector3.up * halfCellSize * (x % 2);
                            Handles.DrawWireDisc(TileToPos(x, y), Vector3.back, cellSize * .66f);
                            //Handles.DrawWireCube(startPos + new Vector3(x * cellSize, y * cellSize, 0) + new Vector3(halfCell, halfCell, 0), (Vector3)Vector2.one * cellSize * 0.75f);
                        }
                    }
                }
            }
#endif

        }
    }
}
