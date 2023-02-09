using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RiverFlow.Core;

namespace RiverFlow.LD
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MapData))]
    public class MapDataInspector : Editor
    {
        SerializedProperty gridSize;
        SerializedProperty typePalette;

        // Start like
        private void OnEnable()
        {
            //Link variables to property
            gridSize = serializedObject.FindProperty(nameof(MapData.size));
            typePalette = serializedObject.FindProperty(nameof(MapData.typePalette));
        }

        // Update like
        public override void OnInspectorGUI()
        {
            //serializedObject copy ma target
            serializedObject.Update();

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(gridSize);
                if (check.changed)
                {
                    //GridSize Min Size (1,1)
                    if (gridSize.vector2IntValue.x < 1) gridSize.vector2IntValue = new Vector2Int(1, gridSize.vector2IntValue.y);
                    if (gridSize.vector2IntValue.y < 1) gridSize.vector2IntValue = new Vector2Int(gridSize.vector2IntValue.x, 1);

                    //target recois serializedObject values (comprend le set dirty et le 
                    serializedObject.ApplyModifiedProperties();
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(typePalette);
                if (GUILayout.Button("UpdateColor", EditorStyles.miniButton))
                {
                    UpdateColor();
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUILayout.Button("Save To Texture", EditorStyles.miniButton, GUILayout.Height(3 * EditorGUIUtility.singleLineHeight)))
            {
                MapData data = (MapData)target;
                data.MapToTextureChannel();
            }

            if (GUILayout.Button("Open Editor Window", EditorStyles.miniButton, GUILayout.Height(3 * EditorGUIUtility.singleLineHeight)))
            {
                OpenWindow();
            }
        }

        private void OpenWindow()
        {
            //Dock to Inspector
            MapDataEditorWindow myWindow;

            if (!EditorWindow.HasOpenInstances<MapDataEditorWindow>())
            {
                myWindow = EditorWindow.CreateWindow<MapDataEditorWindow>("Level Editor Window", new Type[] { typeof(SceneView) });
            }
            else
            {
                myWindow = EditorWindow.GetWindow(typeof(MapDataEditorWindow)) as MapDataEditorWindow;
            }

            myWindow.InitWindow(target as MapData);
            myWindow.Show();
        }

        public void UpdateColor()
        {
            var palette = TileTypePalette.instance;

            typePalette.GetArrayElementAtIndex(0).colorValue = palette.errorMat;
            typePalette.GetArrayElementAtIndex(1).colorValue = palette.grass;
            typePalette.GetArrayElementAtIndex(2).colorValue = palette.clay;
            typePalette.GetArrayElementAtIndex(3).colorValue = palette.aride;
            typePalette.GetArrayElementAtIndex(4).colorValue = palette.mountain;
        }
    }
}
