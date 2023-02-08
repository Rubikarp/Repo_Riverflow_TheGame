using System;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public enum InputMode
    {
        None = 0,
        Dig = 1,
        Erase = 2,
        Cloud = 3,
        Lake = 4,
        Source = 5,
    }

    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        [SerializeField, ReadOnly] private bool isMaintaining = false;
        [SerializeField, ReadOnly] private InputMode mode = InputMode.Dig;
        [SerializeField, ReadOnly] private InputMode modeBeforeErase = InputMode.None;

        public InputMode Mode { get => mode; }

        [Header("Reference")]
        [SerializeField] private GamePlane playableArea;
        [Header("Event")]
        public UnityEvent<InputMode, Vector3> onInputPress;
        public UnityEvent<InputMode, Vector3> onInputMaintain;
        public UnityEvent<InputMode> onInputRelease;
        public UnityEvent<InputMode> onModeChange;
        public UnityEvent<float> onScrollChange;
        public UnityEvent<Vector2> onMoveCam;

        public void Press(Ray ray, bool secondary)
        {
            var hitPoint = playableArea.GetHitPos(ray);
            if (playableArea.InLimit(hitPoint))
            {
                if (secondary)
                {
                    modeBeforeErase = mode;
                    mode = InputMode.Erase;
                }
                isMaintaining = true;
                onInputPress?.Invoke(Mode, hitPoint);
            }
        }
        public void Maintaining(Ray ray, bool secondary)
        {
            var hitPoint = playableArea.GetHitPos(ray);
            if (playableArea.InLimit(hitPoint) && isMaintaining)
            {
                onInputMaintain?.Invoke(Mode, hitPoint);
            }
            else
            {
                if (secondary)
                {
                    mode = modeBeforeErase;
                    modeBeforeErase = InputMode.None;
                }
                isMaintaining = false;
                onInputRelease?.Invoke(Mode);
            }
        }
        public void Release(Ray ray, bool secondary)
        {
            if (secondary && modeBeforeErase != InputMode.None)
            {
                mode = modeBeforeErase;
                modeBeforeErase = InputMode.None;
            }
            isMaintaining = false;
            onInputRelease?.Invoke(Mode);
        }

        public void ModeUpdate(InputMode newMode)
        {
            if (isMaintaining) onInputRelease?.Invoke(Mode);
            if (newMode == mode) return;
            mode = newMode;
            onModeChange?.Invoke(mode);
        }

        public void Scroll(float delta) => onScrollChange?.Invoke(delta);
        public void CamMove(Vector2 deltaMove) => onMoveCam?.Invoke(deltaMove);
    }
}