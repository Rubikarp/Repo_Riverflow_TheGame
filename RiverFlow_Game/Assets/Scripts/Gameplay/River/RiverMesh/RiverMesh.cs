using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RiverFlow.Core
{
    [RequireComponent(typeof(Mesh))]
    [RequireComponent(typeof(MeshFilter))]
    public class RiverMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField, Expandable] private RiverMeshSettings settings;
        private Mesh mesh;

        [SerializeField] private List<RiverPoint> points;

        [Foldout("Mesh Data"), SerializeField, ReadOnly] private int[] tris = new int[12];
        [Foldout("Mesh Data"), SerializeField, ReadOnly] private Vector4[] uvs = new Vector4[6];
        [Foldout("Mesh Data"), SerializeField, ReadOnly] private Vector3[] normals = new Vector3[6];
        [Foldout("Mesh Data"), SerializeField, ReadOnly] private Vector3[] vertices = new Vector3[6];
        [Foldout("Mesh Data"), SerializeField, ReadOnly] private Color[] vertexColors = new Color[6];

        public List<RiverPoint> Points
        {
            get => points;
            private set
            {
                points = value;
                UpdateMesh();
            }
        }
        [SerializeField] private Nullable<RiverPoint> previousPoint = null;
        [SerializeField] private Nullable<RiverPoint> nextPoint = null;


        public void AddPoint(RiverPoint point)
        {
            this.points.Add(point);
            UpdateMesh();
        }
        public void AddPoints(IEnumerable<RiverPoint> points)
        {
            this.points.AddRange(points);
            UpdateMesh();
        }

        public void SetPoints(List<RiverPoint> points)
        {
            float subDiv = 1.0f / (float)settings.subdividePerSegment;
            this.points.Clear();
            for (int i = 0; i < points.Count - 1; i++)
            {
                var prevPoint = points[i];
                var nextPoint = points[i + 1];
                for (int j = 0; j < settings.subdividePerSegment; j++)
                {
                    this.points.Add(RiverPoint.Lerp(prevPoint, nextPoint, j * subDiv));
                }
            }
            this.points.Add(points.Last());
            UpdateMesh();
        }

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();

            meshFilter.mesh = mesh;
            settings = RiverMeshSettings.Instance;
        }

        [Button]
        public void UpdateMesh()
        {
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.Clear();
            }
            else
            {
                mesh.Clear();
            }

            if (points.Count < 2)
            {
                //Error Catching
                Debug.LogError("pas assez de point");
                return;
            }

            #region Compute array size
            int pointCount = points.Count;
            vertexColors = new Color[pointCount * 3];
            vertices = new Vector3[pointCount * 3];
            normals = new Vector3[pointCount * 3];
            uvs = new Vector4[pointCount * 3];
            //
            int segmentCount = points.Count - 1;
            tris = new int[3 * segmentCount * 4];
            #endregion

            #region Vertices
            Vector3 previousDir = (points[1].pos - points[0].pos).normalized;
            if (!(previousPoint is null)) previousDir = (points[0].pos - previousPoint.Value.pos).normalized;
            Vector3 nextDir = (points[1].pos - points[0].pos).normalized;
            Vector3 dir = (previousDir + nextDir) * 0.5f;
            Vector3 right = Vector3.Cross(Vector3.back, dir).normalized;
            float thickness = 0.5f * settings.scale;
            //
            vertices[0] = points[0].pos;
            vertices[1] = points[0].pos - (right * (thickness + (points[0].lake * settings.lakeScale)));
            vertices[2] = points[0].pos + (right * (thickness + (points[0].lake * settings.lakeScale)));

            for (int i = 1; i < pointCount - 1; i++)
            {
                previousDir = (points[i].pos - points[i - 1].pos).normalized;
                nextDir = (points[i + 1].pos - points[i].pos).normalized;
                dir = (previousDir + nextDir) * 0.5f;
                right = Vector3.Cross(Vector3.back, dir).normalized;
                //
                vertices[(i * 3) + 0] = points[i].pos;
                vertices[(i * 3) + 1] = points[i].pos - (right * (thickness + (points[i].lake * settings.lakeScale)));
                vertices[(i * 3) + 2] = points[i].pos + (right * (thickness + (points[i].lake * settings.lakeScale)));
            }

            previousDir = (points[(pointCount - 1)].pos - points[(pointCount - 2)].pos).normalized;
            nextDir = previousDir;
            if (!(nextPoint is null)) nextDir = (nextPoint.Value.pos - points[pointCount - 1].pos).normalized;

            dir = (previousDir + nextDir) * 0.5f;
            right = Vector3.Cross(Vector3.back, dir).normalized;
            //
            vertices[((pointCount - 1) * 3) + 0] = points[pointCount - 1].pos;
            vertices[((pointCount - 1) * 3) + 1] = points[pointCount - 1].pos - (right * (thickness + (points[pointCount - 1].lake * settings.lakeScale)));
            vertices[((pointCount - 1) * 3) + 2] = points[pointCount - 1].pos + (right * (thickness + (points[pointCount - 1].lake * settings.lakeScale)));
            #endregion
            #region Normal
            for (int i = 0; i < pointCount; i++)
            {
                normals[(i * 3) + 0] = Vector3.back;
                normals[(i * 3) + 1] = Vector3.back;
                normals[(i * 3) + 2] = Vector3.back;
            }
            #endregion
            #region Vertex Colors
            Color temp;
            for (int i = 0; i < pointCount; i++)
            {
                temp = points[i].color;
                vertexColors[(i * 3) + 0] = temp;
                vertexColors[(i * 3) + 1] = temp;
                vertexColors[(i * 3) + 2] = temp;
            }
            #endregion
            #region UV
            float distTravell = 0;

            uvs[0] = new Vector4(distTravell, 0.5f, thickness, points[0].lake * settings.lakeScale);
            uvs[1] = new Vector4(distTravell, 1.0f, thickness, points[0].lake * settings.lakeScale);
            uvs[2] = new Vector4(distTravell, 0.0f, thickness, points[0].lake * settings.lakeScale);

            for (int i = 1; i < pointCount; i++)
            {
                distTravell += (points[i].pos - points[i - 1].pos).magnitude;

                uvs[(i * 3) + 0] = new Vector4(distTravell, 0.5f, thickness, points[i].lake * settings.lakeScale);
                uvs[(i * 3) + 1] = new Vector4(distTravell, 1.0f, thickness, points[i].lake * settings.lakeScale);
                uvs[(i * 3) + 2] = new Vector4(distTravell, 0.0f, thickness, points[i].lake * settings.lakeScale);
            }
            #endregion
            #region Triangle
            for (int i = 0; i < pointCount - 1; i++)
            {
                tris[(i * 12) + 00] = (i * 3) + 0;
                tris[(i * 12) + 01] = (i * 3) + 1;
                tris[(i * 12) + 02] = (i * 3) + 3;
                //
                tris[(i * 12) + 03] = (i * 3) + 0;
                tris[(i * 12) + 04] = (i * 3) + 3;
                tris[(i * 12) + 05] = (i * 3) + 2;
                //
                tris[(i * 12) + 06] = (i * 3) + 3;
                tris[(i * 12) + 07] = (i * 3) + 1;
                tris[(i * 12) + 08] = (i * 3) + 4;
                //
                tris[(i * 12) + 09] = (i * 3) + 2;
                tris[(i * 12) + 10] = (i * 3) + 3;
                tris[(i * 12) + 11] = (i * 3) + 5;
            }
            #endregion

            mesh.SetVertices(vertices);
            mesh.SetColors(vertexColors);
            mesh.SetTriangles(tris, 0);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.MarkDynamic();

            meshFilter.mesh = mesh;
        }
        [Button]
        public void UpdateData()
        {
            if (mesh is null || points.Count < 2)
            {
                UpdateMesh();
                return;
            }

            #region Compute array size
            int pointCount = points.Count;
            vertexColors = new Color[pointCount * 3];
            uvs = new Vector4[pointCount * 3];
            #endregion

            #region Vertex Colors
            Color temp;
            for (int i = 0; i < pointCount; i++)
            {
                temp = points[i].color;
                vertexColors[(i * 3) + 0] = temp;
                vertexColors[(i * 3) + 1] = temp;
                vertexColors[(i * 3) + 2] = temp;
            }
            #endregion
            #region UV
            float distTravell = 0;
            float thickness = 0.5f * settings.scale;
            uvs[0] = new Vector4(distTravell, 0.5f, thickness, points[0].lake);
            uvs[1] = new Vector4(distTravell, 1.0f, thickness, points[0].lake);
            uvs[2] = new Vector4(distTravell, 0.0f, thickness, points[0].lake);

            for (int i = 1; i < pointCount; i++)
            {
                distTravell += (points[i].pos - points[i - 1].pos).magnitude;

                uvs[(i * 3) + 0] = new Vector4(distTravell, 0.5f, thickness, points[i].lake);
                uvs[(i * 3) + 1] = new Vector4(distTravell, 1.0f, thickness, points[i].lake);
                uvs[(i * 3) + 2] = new Vector4(distTravell, 0.0f, thickness, points[i].lake);
            }
            #endregion

            mesh.SetColors(vertexColors);
            mesh.SetUVs(0, uvs);
            mesh.MarkDynamic();

            meshFilter.mesh = mesh;
        }

    }

    [System.Serializable]
    public struct RiverPoint
    {
        [Header("Data")]
        public Vector3 pos;
        [ColorUsage(true, false)] public Color color;
        public float lake;


        #region Constructor
        //Vec3 version
        public RiverPoint(Vector3 pos)
        {
            this.pos = pos;
            this.color = Color.white;
            this.lake = 0.0f;
        }
        public RiverPoint(Vector3 pos, Color col, float lake = 0.0f)
        {
            this.pos = pos;
            this.color = col;
            this.lake = lake;
        }
        //Vec2 version
        public RiverPoint(Vector2 pos)
        {
            this.pos = pos;
            this.color = Color.white;
            this.lake = 0.0f;
        }
        public RiverPoint(Vector2 pos, Color col, float lake = 0.0f)
        {
            this.pos = pos;
            this.color = col;
            this.lake = lake;
        }
        #endregion

        public static RiverPoint Lerp(RiverPoint a, RiverPoint b, float t)
        {
            return new RiverPoint(
                Vector3.Lerp(a.pos, b.pos, t),
                Color.Lerp(a.color, b.color, t),
                Mathf.Lerp(a.lake, b.lake, EaseInOutSine(t))
                );
        }
        public static RiverPoint Lerp_EaseInOut(RiverPoint a, RiverPoint b, float t)
        {
            return new RiverPoint(
                Vector3.Lerp(a.pos, b.pos, EaseInOutSine(t)),
                Color.Lerp(a.color, b.color, EaseInOutSine(t)),
                Mathf.Lerp(a.lake, b.lake, EaseInOutSine(t))
                );
        }
        private static float EaseInOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1.0f) / 2.0f;
        }
    }
}
