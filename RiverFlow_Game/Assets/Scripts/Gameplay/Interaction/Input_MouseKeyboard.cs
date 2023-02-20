using System;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using RiverFlow.Core;

namespace RiverFlow.Gameplay.Interaction
{
    public class Input_MouseKeyboard : MonoBehaviour
    {
        [SerializeField, Required] private InputHandler input;

        [Header("Event")]
        [Foldout("Camera")] public UnityEvent<Vector2> onMoveCam;
        [Foldout("Camera")] public UnityEvent<float> onScrollChange;
        [Space(5)]
        [Foldout("Switch")] public UnityEvent<InputMode> onModeChange;
        [Space(5)]
        [Foldout("Click")] public UnityEvent<Ray, bool> onInputPress;
        [Foldout("Click")] public UnityEvent<Ray, bool> onInputMaintain;
        [Foldout("Click")] public UnityEvent<Ray, bool> onInputRelease;

        [Header("Mode")]
        [SerializeField] ModeKeyMapping keyMode;
        public float scrollSensitivity = 1f;
        public float moveSensitivity = .1f;

        [Header("Internal Value")]
        [SerializeField, ReadOnly] bool isMaintainingL = false;
        [SerializeField, ReadOnly] bool isMaintainingR = false;
        [SerializeField, ReadOnly] bool isMaintainingM = false;
        [SerializeField] private InteractionPlane Limit => input.playableArea;
        public Ray MouseRay { get => Utilities_UI.MouseScreenRay(); }

        private void Update()
        {
            CheckMode();
            CheckInput();
        }

        private void CheckInput()
        {
            //OnPress
            if (Input.GetMouseButtonDown(0))
            {
                if (Utilities_UI.IsOverUI()) return;
                if (Limit.MouseInLimit())
                {
                    isMaintainingL = true;
                    onInputPress?.Invoke(MouseRay, false);
                }
            }
            //OnDrag
            if (Input.GetMouseButton(0))
            {
                if (!Limit.MouseInLimit() || Utilities_UI.IsOverUI())
                {
                    isMaintainingL = false;
                    onInputRelease?.Invoke(MouseRay, false);
                }
                else
                if (isMaintainingL)
                {
                    onInputMaintain?.Invoke(MouseRay, false);
                }
            }
            //OnRelease
            if (Input.GetMouseButtonUp(0))
            {
                if (isMaintainingL)
                {
                    isMaintainingL = false;
                    onInputRelease?.Invoke(MouseRay, false);
                }
            }

            //Right Click eraser
            //OnPress
            if (Input.GetMouseButtonDown(1))
            {
                if (Utilities_UI.IsOverUI()) return;
                if (Limit.MouseInLimit())
                {
                    isMaintainingR = true;
                    onInputPress?.Invoke(MouseRay, true);
                }
            }
            //OnDrag
            if (Input.GetMouseButton(1))
            {
                if (!Limit.MouseInLimit() || Utilities_UI.IsOverUI())
                {
                    isMaintainingR = false;
                    onInputRelease?.Invoke(MouseRay, true);
                }
                else
                if (isMaintainingR)
                {
                    onInputMaintain?.Invoke(MouseRay, true);
                }
            }
            //OnRelease
            if (Input.GetMouseButtonUp(1))
            {
                if (isMaintainingR)
                {
                    isMaintainingR = false;
                    onInputRelease?.Invoke(MouseRay, true);
                }
            }


            //OnPress
            if (Input.GetMouseButtonDown(2))
            {
                    isMaintainingM = true;
            }
            //OnDrag
            if (Input.GetMouseButton(2))
            {
                if (isMaintainingM)
                {
                    onMoveCam?.Invoke(Extension_Mouse.mouseDelta * moveSensitivity * Time.deltaTime);
                }
            }
            //OnRelease
            if (Input.GetMouseButtonUp(2))
            {
                if (isMaintainingM)
                {
                    isMaintainingM = false;
                }
            }

            if (Input.mouseScrollDelta.y * scrollSensitivity != 0)
            {
                onScrollChange?.Invoke(Input.mouseScrollDelta.y * scrollSensitivity);
            }
        }
        private void CheckMode()
        {
            if (Input.GetKeyDown(keyMode.dig)) onModeChange?.Invoke(InputMode.Dig);
            else 
            if (Input.GetKeyDown(keyMode.eraser)) onModeChange?.Invoke(InputMode.Erase);
            else 
            if (Input.GetKeyDown(keyMode.cloud)) onModeChange?.Invoke(InputMode.Cloud);
            else 
            if (Input.GetKeyDown(keyMode.lake)) onModeChange?.Invoke(InputMode.Lake);
            else 
            if (Input.GetKeyDown(keyMode.source)) onModeChange?.Invoke(InputMode.Source);
        }
    }

    [Serializable]
    public class ModeKeyMapping
    {
        public KeyCode dig      = KeyCode.Alpha1;
        public KeyCode eraser   = KeyCode.Alpha2;
        public KeyCode cloud    = KeyCode.Alpha3;
        public KeyCode lake     = KeyCode.Alpha4;
        public KeyCode source   = KeyCode.Alpha5;
    }
}