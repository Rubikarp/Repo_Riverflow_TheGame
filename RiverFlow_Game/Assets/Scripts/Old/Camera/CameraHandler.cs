using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraHandler : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] GamePlane playbleArea;
        [SerializeField] GamePlane cameraArea;
        [SerializeField, MinMaxSlider(0, 64)] Vector2 zoomBoundary = new Vector2(8, 32);
        public float ZoomLvlMin { get => zoomBoundary.x; }
        public float ZoomLvlMax { get => zoomBoundary.y; }
        public float ZoomSize { get => 2f * cam.orthographicSize; set => cam.orthographicSize = value * 0.5f; }
        public Vector2 ViewSize { get => new Vector2(ZoomSize * cam.aspect, ZoomSize); }

        private void Awake()
        {
            cam = transform.GetComponent<Camera>();
        }

        public void Move(Vector2 deltaMove)
        {
            //playbleArea.InLimit(transform.position + (Vector3)deltaMove);
            Debug.Log(deltaMove);
            transform.position -= (Vector3)deltaMove;
        }

        public void Zoom(float delta)
        {
            ZoomSize = Mathf.Clamp(ZoomSize + delta, ZoomLvlMin, ZoomLvlMax);
        }
    }
}
