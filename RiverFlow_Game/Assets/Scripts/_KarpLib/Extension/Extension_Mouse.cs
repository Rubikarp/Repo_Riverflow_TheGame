using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public static class Extension_Mouse 
    {
        private static Vector2 currentMousePos;
        private static Vector2 lastMousePos;
        private static Vector2 currentDelta;
        private static Vector2[] dragStartPos = new Vector2[3];
        private static Vector2[] dragVector = new Vector2[3];
        private static int lastFrame = -1;

        public static Vector2 mousePosition { get { Update(); return currentMousePos; } }
        public static Vector2 lastMousePosition { get { Update(); return lastMousePos; } }
        public static Vector2 mouseDelta { get { Update(); return currentDelta; } }
        public static Vector2 GetDragStartPoint(int mouseIndex) { Update(); return dragStartPos[mouseIndex]; }
        public static Vector2 GetDragOffset(int mouseIndex) { Update(); return dragVector[mouseIndex]; }

        static Extension_Mouse()
        {
            // force initialization on first access
            currentMousePos = Input.mousePosition;
            Update();
            lastFrame = -1;
        }

        static void Update()
        {
            if (lastFrame >= Time.frameCount) return;
            if (lastFrame < Time.frameCount - 1) currentMousePos = Input.mousePosition;

            lastFrame = Time.frameCount;
            lastMousePos = currentMousePos;
            currentMousePos = Input.mousePosition;
            currentDelta = currentMousePos - lastMousePos;

            for (int i = 0; i < dragStartPos.Length; i++)
            {
                if (Input.GetMouseButtonDown(i)) dragStartPos[i] = currentMousePos;
                if (Input.GetMouseButton(i)) dragVector[i] = currentMousePos - dragStartPos[i];
            }
        }
    }

}
