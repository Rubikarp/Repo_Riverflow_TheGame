#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public static class Extension_Handles
{
    public static void DrawWireSquare(Vector3 center, Vector2 size, float thickneess = 1f)
    {
        Vector3 halfSize = ((Vector3)size)/2;
        Handles.DrawLine(center + Vector3.Scale(halfSize, new Vector2(-1, 1)), center + halfSize, thickneess);
        Handles.DrawLine(center + Vector3.Scale(halfSize, new Vector2(1, -1)), center + halfSize, thickneess);
        Handles.DrawLine(center + Vector3.Scale(halfSize, new Vector2(1, -1)), center - halfSize, thickneess);
        Handles.DrawLine(center + Vector3.Scale(halfSize, new Vector2(-1, 1)), center - halfSize, thickneess);
    }

}
#endif
