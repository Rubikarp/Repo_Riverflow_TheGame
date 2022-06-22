using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraPlane : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField, OnValueChanged("InitScreenArea")] float dist = 10;

    [Header("Boundary")]
    [SerializeField, ReadOnly] Bounds area = new Bounds(Vector2.zero, Vector2.one);
    public Bounds ScreenArea { get => area; }

    [Header("Surface")]
    [SerializeField, ReadOnly] Plane plane = new Plane(Vector3.back, Vector3.zero);
    public Plane Surface { get => plane; }

    [SerializeField, Foldout("Debug")] Color debugColor = Color.green;
    [SerializeField, Foldout("Debug"), ReadOnly] Vector3 hitPoint = Vector3.zero;
    [SerializeField, Foldout("Debug"), ReadOnly] float hitDist = 0f;
    [SerializeField, Foldout("Debug"), ReadOnly] Ray ray;

    public void SetDepth(float depth)
    {
        this.dist = depth;
        InitScreenArea();
    }

    [Button]
    private void InitScreenArea()
    {
        var cam = Utilities_UI.Camera;
        float frustumHeight;

        area.center = cam.transform.position + cam.transform.forward * dist;
        frustumHeight = cam.orthographic ? cam.orthographicSize : Mathf.Abs(dist) * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumHeight *= 2f;
        area.size = new Vector3(frustumHeight * cam.aspect, frustumHeight, 0f);

        plane = new Plane(Vector3.back, area.center);
    }

    public Vector3 GetMouseHitPos()
    {
        ray = Utilities_UI.MouseScreenRay();
        return GetHitPos(ray);
    }
    public Vector3 GetHitPos(Ray ray)
    {
        //Reset HitPoint
        hitPoint = Vector3.zero;

        //Raycast
        plane.Raycast(ray, out hitDist);
        if (hitDist <= 0)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 0.1f);
            if (hitDist == 0)
            {
                Debug.LogError("Ray parrallele to plane");
            }
            else
            {
                Debug.LogWarning("Ray hit the plane backward");
            }
        }
        hitPoint = ray.GetPoint(hitDist);

        return hitPoint;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        using (new Handles.DrawingScope(debugColor))
        {
            Extension_Handles.DrawWireSquare(area.center, area.size);
        }
#endif
    }
}
