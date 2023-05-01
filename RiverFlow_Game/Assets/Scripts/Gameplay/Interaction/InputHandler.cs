using System;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace RiverFlow.Gameplay.Interaction
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
        [SerializeField, ReadOnly] private Vector3 hitPoint;
        [SerializeField, ReadOnly] private InputMode mode = InputMode.Dig;
        [SerializeField, ReadOnly] private InputMode modeBeforeErase = InputMode.None;

        public InputMode Mode { get => mode; }

        [Header("Reference")]
        [SerializeField] public InteractionPlane playableArea;

        [Header("Event")]
        [Foldout("Camera")] public UnityEvent<float> onScrollChange;
        [Foldout("Camera")] public UnityEvent<Vector2> onMoveCam;
        [Space(5)]
        [Foldout("Switch")] public UnityEvent<InputMode> onModeChange;
        [Space(5)]
        [Foldout("Interaction")] public UnityEvent<InputMode, Vector3> onInputPress = new UnityEvent<InputMode, Vector3>();
        [Foldout("Interaction")] public UnityEvent<InputMode, Vector3> onInputMaintain = new UnityEvent<InputMode, Vector3>();
        [Foldout("Interaction")] public UnityEvent<InputMode, Vector3> onInputRelease = new UnityEvent<InputMode, Vector3>();

        public void Press(Ray ray, bool secondary)
        {
            hitPoint = playableArea.GetHitPos(ray);
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
            hitPoint = playableArea.GetHitPos(ray);
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
                onInputRelease?.Invoke(Mode, hitPoint);
            }
        }
        public void Release(Ray ray, bool secondary)
        {
            if (isMaintaining)
            {
                if (secondary && modeBeforeErase != InputMode.None)
                {
                    mode = modeBeforeErase;
                    modeBeforeErase = InputMode.None;
                }
                isMaintaining = false;
                onInputRelease?.Invoke(Mode, hitPoint);
            }
        }

        public void ModeUpdate(InputMode newMode)
        {
            if (isMaintaining) onInputRelease?.Invoke(Mode,hitPoint);
            if (newMode == mode) return;
            mode = newMode;
            onModeChange?.Invoke(mode);
        }

        public void Scroll(float delta) => onScrollChange?.Invoke(delta);
        public void CamMove(Vector2 deltaMove) => onMoveCam?.Invoke(deltaMove);
    }
}