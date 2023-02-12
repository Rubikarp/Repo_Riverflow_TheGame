using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.Gameplay.Interaction
{
    public class InteractionPlane : MonoBehaviour
    {
        #region Debug
        [SerializeField, Foldout("Debug")] Color debugColor = Color.green;
        [SerializeField, Foldout("Debug"), ReadOnly] Vector3 hitPoint = Vector3.zero;
        [SerializeField, Foldout("Debug"), ReadOnly] float hitDist = 0f;
        [SerializeField, Foldout("Debug"), ReadOnly] Ray ray;
        #endregion

        [Header("Parameter")]
        [SerializeField, OnValueChanged("InitArea")] Vector2 offSet = Vector2.zero;
        [SerializeField, OnValueChanged("InitArea")] Vector2 size = Vector2.one * 2;
        [SerializeField, ReadOnly] Bounds area = new Bounds(Vector2.zero, Vector2.one);
        [SerializeField, ReadOnly] Plane plane = new Plane(Vector3.back, Vector3.zero);
        public Bounds Area => area;
        public Plane Plane => plane;

        [Button]
        private void InitArea()
        {
            area.center = transform.position + (Vector3)offSet;
            area.size = new Vector3(size.x, size.y, 0f);

            plane = new Plane(Vector3.back, area.center);
        }

        public void SetLimit(Vector2 size) => SetLimit(size, Vector2.zero);
        public void SetLimit(Vector2 size, Vector2 offSet)
        {
            this.size = size;
            this.offSet = offSet;
            InitArea();
        }

        public bool InLimit(Vector3 hitPos) => area.Contains(hitPos);
        public bool MouseInLimit() => area.Contains(GetMouseHitPos());

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
        public Vector3 GetMouseHitPos() => GetHitPos(Utilities_UI.MouseScreenRay());


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            using (new Handles.DrawingScope(debugColor))
            {
                Extension_Handles.DrawWireSquare(area.center, area.size, 5f);
            }
        }
#endif
    }
}
