using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utilities_UI
{
    //Camera Keep
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }
    public static Vector3 CameraPos
    {
        get
        {
            return Camera.transform.position;
        }
    }

    //World to UI
    public static Vector3 GetWorldPosOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    //Mouse OverUI ?
    private static PointerEventData _eventDataCurrentPos;
    private static List<RaycastResult> _results;
    public static bool IsOverUI()
    {
        _eventDataCurrentPos = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPos, _results);
        return _results.Count > 0;
    }

    public static bool MouseInViewPort()
    {
        Rect viewPort = new Rect(Vector2.zero, Vector2.one);
        Vector2 viewportPos = Camera.ScreenToViewportPoint(Input.mousePosition);
        return viewPort.Contains(viewportPos);
    }

    public static Ray MouseScreenRay()
    {
        return Utilities_UI.Camera.ScreenPointToRay(Input.mousePosition);
    }

}
