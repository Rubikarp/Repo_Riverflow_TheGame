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
    [Serializable] public class InputBrutEvent : UnityEvent<Ray, bool> { }
    [Serializable] public class InputModeEvent : UnityEvent<InputMode> { }
    [Serializable] public class InputEvent : UnityEvent<InputMode, Vector3> { }

    public class InputHandler : SingletonMonoBehaviour<InputHandler>
    {
        [Header("Internal Value")]
        [SerializeField, ReadOnly] InputMode modeBeforeErase = InputMode.None;
        public InputMode mode = InputMode.Dig;
        public InputMode Mode
        {
            get => mode;
            set
            {
                if (isMaintaining) onInputRelease?.Invoke(Mode);
                mode = value;
            }
        }
        [Space(10)]
        [SerializeField] GamePlane playableArea;

        [SerializeField, ReadOnly] bool isMaintaining = false;

        [Header("Event")]
        public InputEvent onInputPress;
        public InputEvent onInputMaintain;
        public InputModeEvent onInputRelease;
        public UnityEvent<float> onScrollChange;
        public UnityEvent<Vector2> onMoveCam;

        public void ModeUpdate(InputMode newMode) => Mode = newMode;

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

        public void Scroll(float delta )
        {
            onScrollChange?.Invoke(delta);
        }
        public void CamMove(Vector2 deltaMove)
        {
            onMoveCam?.Invoke(deltaMove);
        }
    }
}