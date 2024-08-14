using UnityEngine;
using NaughtyAttributes;
using RiverFlow.Gameplay.Interaction;
using DG.Tweening;

namespace RiverFlow.Core
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Camera cam;
        [SerializeField] private InteractionPlane playbleArea;
        [SerializeField] private InteractionPlane cameraArea;
        [SerializeField, MinMaxSlider(0, 64)] private Vector2 zoomBoundary = new Vector2(8, 32);
        public float ZoomLvlMin => zoomBoundary.x;
        public float ZoomLvlMax => zoomBoundary.y;
        public Vector2 ViewSize => new Vector2(ZoomSize * cam.aspect, ZoomSize);
        public float ZoomSize { get => 2f * cam.orthographicSize; private set => cam.orthographicSize = value * 0.5f; }

        private void Awake() => cam = Camera.main;
        

        public void Zoom(float delta) => ZoomSize = Mathf.Clamp(ZoomSize - delta, ZoomLvlMin, ZoomLvlMax);
        public void Move(Vector2 deltaMove)
        {
            cam.transform.position -= (Vector3)deltaMove;

            if (!playbleArea.Area.Contains(cam.transform.position))
            {
                cam.transform.position = playbleArea.Area.ClosestPoint(cam.transform.position);
                cam.transform.position += Vector3.back * 10;
            }
        } 
    }
}
