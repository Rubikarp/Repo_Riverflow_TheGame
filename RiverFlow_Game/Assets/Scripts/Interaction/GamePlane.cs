using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.Core
{
    public class GamePlane : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField, OnValueChanged("InitPlayableArea")] Vector2 offSet = Vector2.zero;
        [SerializeField, OnValueChanged("InitPlayableArea")] Vector2 size = Vector2.one * 2;
        public void SetLimit(Vector2 size, Vector2 offSet = new Vector2())
        {
            this.size = size;
            this.offSet = offSet;
            InitPlayableArea();
        }

        [Header("Boundary")]
        [SerializeField, ReadOnly] Bounds area = new Bounds(Vector2.zero, Vector2.one);
        public Bounds Area { get => area; }

        [Header("Surface")]
        [SerializeField, ReadOnly] Plane plane = new Plane(Vector3.back, Vector3.zero);
        public Plane Plane { get => plane; }

        [SerializeField, Foldout("Debug")] Color debugColor = Color.green;
        [SerializeField, Foldout("Debug"), ReadOnly] Vector3 hitPoint = Vector3.zero;
        [SerializeField, Foldout("Debug"), ReadOnly] float hitDist = 0f;
        [SerializeField, Foldout("Debug"), ReadOnly] Ray ray;

        [Button]
        private void InitPlayableArea()
        {
            area.center = transform.position + (Vector3)offSet;
            area.size = new Vector3(size.x, size.y, 0f);

            plane = new Plane(Vector3.back, area.center);
        }

        public bool MouseInLimit()
        {
            return area.Contains(GetMouseHitPos());
        }

        public bool InLimit(Vector3 hitPos)
        {
            return area.Contains(hitPos);
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
}
