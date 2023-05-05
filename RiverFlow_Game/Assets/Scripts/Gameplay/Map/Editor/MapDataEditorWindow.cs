using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RiverFlow.Core;

namespace RiverFlow.LD
{
    public class MapDataEditorWindow : EditorWindow
    {
        MapData currentLD;
        SerializedObject serializedObject;
        //
        SerializedProperty topoTypeProp;
        SerializedProperty sizeProp;
        int width, height;
        //
        SerializedProperty tileColorProp;
        SerializedProperty selectedTileType;
        //
        Rect rectIn;
        bool isClicking;
        int brushSize = 3;
        float marginRatio;
        Vector2 mousePos, scrollPos;

        public void InitWindow(MapData _currentLD)
        {
            currentLD = _currentLD;
            serializedObject = new SerializedObject(currentLD);

            //Link variables to property
            topoTypeProp = serializedObject.FindProperty(nameof(MapData.topology));

            sizeProp = serializedObject.FindProperty(nameof(MapData.size));
            width = sizeProp.vector2IntValue.x;
            height = sizeProp.vector2IntValue.y;

            tileColorProp = serializedObject.FindProperty(nameof(MapData.typePalette));
            selectedTileType = serializedObject.FindProperty(nameof(MapData.selectedType));

            marginRatio = 0.1f;
        }

        //GUI is call 2 time per frame and is null on the second frame
        void OnGUI()
        {
            ProcessEvent();
            serializedObject.Update();
            width = sizeProp.vector2IntValue.x;
            height = sizeProp.vector2IntValue.y;

            //Info
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.ObjectField("Edited Map : ", currentLD, typeof(MapData));
                //Debug
                //EditorGUILayout.Vector2Field("Debug Mouse Pos", Event.current.mousePosition);
            }
            EditorGUILayout.Space(1 * EditorGUIUtility.singleLineHeight);
            //Show actual Brush
            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUILayout.PropertyField(selectedTileType);
                    using (new GUILayout.HorizontalScope())
                    {
                        Rect zone = EditorGUILayout.GetControlRect();
                        EditorGUI.DrawRect(zone, tileColorProp.GetArrayElementAtIndex(selectedTileType.enumValueIndex).colorValue);
                    }
                }
            }
            //Choose Brush
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Herbe", EditorStyles.miniButton)) selectedTileType.enumValueIndex = 1;
                if (GUILayout.Button("Argil", EditorStyles.miniButton)) selectedTileType.enumValueIndex = 2;
                if (GUILayout.Button("Desert", EditorStyles.miniButton)) selectedTileType.enumValueIndex = 3;
                if (GUILayout.Button("Mountain", EditorStyles.miniButton)) selectedTileType.enumValueIndex = 4;
            }
            //Fill Option
            if (GUILayout.Button("Fill grid by selected Brush", EditorStyles.miniButton))
            {
                for (int x = 0; x < currentLD.Size.x; x++)
                    for (int y = 0; y < currentLD.Size.y; y++)
                    {
                        topoTypeProp.GetArrayElementAtIndex(x + y * currentLD.Size.x).enumValueIndex = selectedTileType.enumValueIndex;
                    }
            }
            //edit Brush
            using (new GUILayout.HorizontalScope())
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    EditorGUILayout.PropertyField(sizeProp);
                    if (check.changed)
                    {
                        //GridSize Min Size (1,1)
                        if (sizeProp.vector2IntValue.x < 1) sizeProp.vector2IntValue = new Vector2Int(1, sizeProp.vector2IntValue.y);
                        if (sizeProp.vector2IntValue.y < 1) sizeProp.vector2IntValue = new Vector2Int(sizeProp.vector2IntValue.x, 1);

                        width = sizeProp.vector2IntValue.x;
                        height = sizeProp.vector2IntValue.y;

                        //target recois serializedObject values (comprend le set dirty et le 
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(currentLD);
                    }
                }
            }
            brushSize = EditorGUILayout.IntSlider(brushSize, 1, 11);
            EditorGUILayout.Space(1 * EditorGUIUtility.singleLineHeight);

            #region Edit Area
            //Error Check
            if (currentLD.Size.x < 0) return;
            if (currentLD.Size.y < 0) return;
            //Grid Size check
            if (topoTypeProp.arraySize != width * height) topoTypeProp.arraySize = width * height;

            //Define Area
            Rect nextRect = EditorGUILayout.GetControlRect();
            float totalWidth = EditorGUIUtility.currentViewWidth;

            float border = totalWidth * marginRatio;
            float gridWidth = totalWidth - (2f * border);

            Rect gridArea = new Rect(nextRect.x + border, nextRect.y, gridWidth, gridWidth * ((float)height / (float)width));

            float maxHeightAvailable = position.height - nextRect.y - (2 * EditorGUIUtility.singleLineHeight);
            float visibleAreaHeight = Mathf.Min(gridArea.height, maxHeightAvailable);

            Rect visibleArea = new Rect(gridArea.x, nextRect.y, gridArea.width + 15, visibleAreaHeight);

            //Debug
            EditorGUI.DrawRect(visibleArea, new Color(1f, 1f, 0f, 0.1f));
            EditorGUI.DrawRect(gridArea, new Color(0.5f, 0.5f, 0.5f, 0.2f));

            scrollPos = GUI.BeginScrollView(visibleArea, scrollPos, gridArea);
            using (new GUILayout.VerticalScope())
            {
                #region Draw Grid
                int spaceBtwCell = 4;
                float totalCellWidth = gridWidth + (width * spaceBtwCell);

                float cellWidth = totalCellWidth / (float)width;
                float totalSpaceWitdh = gridWidth - totalCellWidth;

                float spaceWidth = totalSpaceWitdh / ((float)width + 1);

                float curY = gridArea.y;
                for (int y = 0; y < currentLD.Size.y; y++)
                {
                    curY += spaceWidth;

                    float curX = gridArea.x;
                    for (int x = 0; x < currentLD.Size.x; x++)
                    {
                        curX += spaceWidth;
                        Rect rect = new Rect(curX, curY, cellWidth, cellWidth);
                        curX += cellWidth;
                        int tileIndex = x + ((currentLD.Size.y - 1 - y) * (currentLD.Size.x - 1));

                        //Utilisateur peint
                        if (nextRect.y != 0)
                        {
                            if (rect.Contains(mousePos + scrollPos))
                            {
                                if (isClicking)
                                {
                                    int clikedIndex = x + y * currentLD.Size.x;
                                    for (int xx = -Mathf.FloorToInt((float)brushSize / 2f); xx < Mathf.CeilToInt((float)brushSize / 2f); xx++)
                                    {
                                        for (int yy = -Mathf.FloorToInt((float)brushSize / 2f); yy < Mathf.CeilToInt((float)brushSize / 2f); yy++)
                                        {
                                            if (y + yy < 0 || sizeProp.vector2IntValue.y <= y + yy) continue;
                                            if (x + xx < 0 || sizeProp.vector2IntValue.x <= x + xx) continue;
                                            clikedIndex = (x + xx) + (y + yy) * currentLD.Size.x;
                                            topoTypeProp.GetArrayElementAtIndex(clikedIndex).enumValueIndex = selectedTileType.enumValueIndex;
                                        }
                                    }

                                    //topoTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex = selectedTileType.enumValueIndex;

                                    int debugBorder = 5;
                                    Rect debugRect = new Rect(rect.x - debugBorder, rect.y - debugBorder, rect.width + 2 * debugBorder, rect.height + 2 * debugBorder);
                                    //EditorGUI.DrawRect(debugRect, Color.magenta);

                                    // Debug.Log("The tile : " + x + " " + y + " " + "in rect " + rect + "have value changed" + " at pose " + mousePos);
                                }
                            }
                        }

                        //Draw tile
                        int enumIndexPalette = topoTypeProp.GetArrayElementAtIndex(tileIndex).enumValueIndex;
                        Color rendColor = tileColorProp.GetArrayElementAtIndex(enumIndexPalette).colorValue;
                        EditorGUI.DrawRect(rect, rendColor);
                        //EditorGUI.LabelField(rect, "Pos" + rect.x + "/" + rect.y + "\n Size" + rect.width + "/" + rect.height);

                    }
                    curY += cellWidth;
                }

                Vector2 mousePencilPos = mousePos + scrollPos;
                float pencilSize = cellWidth * (brushSize - 0.5f);
                Rect pencilRect = new Rect(mousePencilPos.x - (.5f * pencilSize), mousePencilPos.y - (.5f * pencilSize), pencilSize, pencilSize);
                EditorGUI.DrawRect(pencilRect, new Color(0, 0, 0, .25f));

                #endregion
            }
            // End the scroll view that we began above.
            GUI.EndScrollView();
            GUILayout.Space(visibleArea.height);
            #endregion

            Repaint();
            serializedObject.ApplyModifiedProperties();
        }

        void ProcessEvent()
        {
            mousePos = Event.current.mousePosition;

            if (Event.current.type == EventType.MouseDown) isClicking = true;
            if (Event.current.type == EventType.MouseUp) isClicking = false;
        }
    }
}