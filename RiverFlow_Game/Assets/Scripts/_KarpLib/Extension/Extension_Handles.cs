#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public static class Extension_Handles
{
    public static void DrawWireSquare(Vector3 center, Vector2 size)
    {
        var drawOrigin = center - ((Vector3)size) * .5f;

        Handles.DrawLines(
        new Vector3[]
        {
            drawOrigin,
            drawOrigin + Vector3.up * size.y,
            drawOrigin + ((Vector3)size),
            drawOrigin + Vector3.right * size.x,
        }, 
        new int[]
        {
            0,1,1,2,2,3,3,0
        });
    }
    public static void DrawWireSquare(Vector3 center, Vector2 size, float thickness)
    {
        var drawOrigin = center - ((Vector3)size) * .5f;

        Handles.DrawAAPolyLine(thickness,
        new Vector3[]
        {
            drawOrigin,
            drawOrigin + Vector3.up * size.y,
            drawOrigin + ((Vector3)size),
            drawOrigin + Vector3.right * size.x,
            drawOrigin,
        });
    }
    public static void DrawWireSquareGrid(Vector3 center, Vector2Int size, float cellSize, Vector2 cellShape)
    {
        var drawOrigin = center;
        drawOrigin -= (Vector3)Vector2.Scale(-Vector2.one, (Vector2)size / 2f);
        drawOrigin -= (Vector3)cellShape * .5f;

        var squarePos = new Vector3[]
        {
            Vector3.zero,
            Vector3.up * cellShape.y,
            (Vector3)cellShape,
            Vector3.right * cellShape.x,
        };
        var pointsArray = new Vector3[4 * size.x * size.y];

        var squareIndex = new int[]
        {
            0,1,1,2,2,3,3,0
        };
        var indexsArray = new int[8 * size.x * size.y];

        Vector3 currentSquareOrigin;
        int squareID, currentSquareIndex;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                squareID = x + y * size.x;
                currentSquareOrigin = drawOrigin + new Vector3(x * cellSize, y * cellSize, 0);

                for (int i = 0; i < 4; i++)
                {
                    pointsArray[(squareID * 4) + i] = squarePos[i] + currentSquareOrigin;
                }
                for (int i = 0; i < 8; i++)
                {
                    indexsArray[(squareID * 8) + i] = squareIndex[i] + (squareID * 8);
                }
            }
        }

        Handles.DrawLines(pointsArray, indexsArray);

        //Handles.DrawAAPolyLine(thickness * 10, );
    }
}
#endif
