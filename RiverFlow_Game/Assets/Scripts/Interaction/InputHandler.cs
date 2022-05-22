using System;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public enum InputMode
{
    None = 0,
    Dig = 1,
    Erase = 2,
    Cloud = 3,
    Lake = 4,
    Source = 5,
}
[Serializable] public class InputEvent : UnityEvent<InputMode> { }

public class InputHandler : Singleton<InputHandler>
{
    [Header("Event")]
    public InputEvent onInputPress;
    public InputEvent onInputMaintain;
    public InputEvent onInputRelease;

    [Header("Internal Value")]
    Plane inputSurf = new Plane(Vector3.back, Vector3.zero);
    Rect viewport = new Rect(0, 0, 1, 1);
    Ray ray;
    //
    private Camera cam
    {
        get
        {
            if (_camera is null) _camera = KarpHelper.Camera;
            return _camera;
        }
    }
    [SerializeField] Camera _camera;
    //
    [SerializeField, ReadOnly] float hitDist = 0f;
    [SerializeField, ReadOnly] Vector3 hitPoint = Vector3.zero;

    [Header("Mode")]
    public ModeKeyMapping keyMode;

    [Space(10)]
    public InputMode mode = InputMode.Dig;
    public InputMode Mode 
    { 
        get { return mode;}
        set 
        {
            //Change ?
            if (value == mode) return;
            if (!isMaintaining) 
            { 
                mode = value; 
            }
            else
            {
                onInputRelease?.Invoke(mode);
                mode = value;
                onInputPress?.Invoke(mode);
            }
        }
    }
    //public InputMode secondaryMode = InputMode.eraser;

    [SerializeField, ReadOnly] bool isMaintaining = false;
    [SerializeField, ReadOnly] bool shortCutErasing = false;

    //Methods
    public Vector3 GetHitPos()
    {
        //Reset HitPoint
        hitPoint = Vector3.zero;
        //Get Ray
        ray = cam.ScreenPointToRay(Input.mousePosition);
        //Raycast
        if (inputSurf.Raycast(ray, out hitDist))
        {
            hitPoint = ray.GetPoint(hitDist);
        }
        else
        {
            Debug.LogError("Ray parrallèle to plane", this);
        }
        return hitPoint;
    }
    public bool CursorInViewPort()
    {
        Vector2 viewportPos = cam.ScreenToViewportPoint(Input.mousePosition);
        return viewport.Contains(viewportPos);
    }
    public void ChangeMode(InputMode newMode)
    {
        mode = newMode;
    }
    public void ChangeMode(int newMode)
    {
        newMode %= Enum.GetNames(typeof(InputMode)).Length;
        mode = (InputMode)newMode;
    }

    //
    void Update()
    {
        CheckMode();
        CheckInput();
    }
    private void CheckInput()
    {
        //OnPress
        if (Input.GetMouseButtonDown(0))
        {
            if (!KarpHelper.IsOverUI())
            {
                isMaintaining = true;
                onInputPress?.Invoke(mode);
            }
        }
        //OnDrag
        if (Input.GetMouseButton(0))
        {
            if (KarpHelper.IsOverUI())
            {
                onInputRelease?.Invoke(mode);
            }
            else 
            if(isMaintaining)
            {
                onInputMaintain?.Invoke(mode);
            }
        }
        //OnRelease
        if (Input.GetMouseButtonUp(0))
        {
            if (isMaintaining)
            {
                isMaintaining = false;
                onInputRelease?.Invoke(mode);
            }
        }

        //Right Click eraser
        //OnPress
        if (Input.GetMouseButtonDown(1))
        {
            if (!KarpHelper.IsOverUI())
            {
                shortCutErasing = true;
                isMaintaining = true;
                onInputPress?.Invoke(InputMode.Erase);
            }
        }
        //OnDrag
        if (Input.GetMouseButton(1))
        {
            if (KarpHelper.IsOverUI())
            {
                shortCutErasing = false;
                onInputRelease?.Invoke(InputMode.Erase);
            }
            else
            if (isMaintaining)
            {
                shortCutErasing = true;
                onInputMaintain?.Invoke(InputMode.Erase);
            }
        }
        //OnRelease
        if (Input.GetMouseButtonUp(1))
        {
            if (isMaintaining)
            {
                shortCutErasing = false;
                isMaintaining = false;
                onInputRelease?.Invoke(InputMode.Erase);
            }
        }
    }
    private void CheckMode()
    {
        if (Input.GetKeyDown(keyMode.dig))
        {
            mode = InputMode.Dig;
        }
        else 
        if (Input.GetKeyDown(keyMode.eraser))
        {
            mode = InputMode.Erase;
        }
        else
        if (Input.GetKeyDown(keyMode.cloud))
        {
            mode = InputMode.Cloud;
        }
        else
        if (Input.GetKeyDown(keyMode.lake))
        {
            mode = InputMode.Lake;
        }
        else
        if (Input.GetKeyDown(keyMode.source))
        {
            mode = InputMode.Source;
        }
    }
}

[Serializable]
public class ModeKeyMapping
{
    public KeyCode dig = KeyCode.Alpha1;
    public KeyCode eraser = KeyCode.Alpha2;
    public KeyCode cloud = KeyCode.Alpha3;
    public KeyCode lake = KeyCode.Alpha4;
    public KeyCode source = KeyCode.Alpha5;
}
