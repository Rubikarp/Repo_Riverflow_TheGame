using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Parameter")]
    public bool hasResponse;
    public bool isPressed;
    [SerializeField] float holdTime = 0.5f;
    [ProgressBar("holdLvl", "holdTime", EColor.Red), SerializeField] float holdLvl = 0.0f;
    public float fillRatio { get => holdLvl / holdTime; }
    [Header("Event")]
    public UnityEvent onButtonHold;

    private void OnEnable()
    {
        holdLvl = 0.0f;
        hasResponse = false;
    }
    private void OnDisable()
    {
        isPressed = false;
    }

    private void Update()
    {
        if (hasResponse) return;

        holdLvl += isPressed ? Time.deltaTime : -1 * Time.deltaTime;
        holdLvl = Mathf.Clamp(holdLvl, 0, holdTime);

        if (holdLvl >= holdTime && isPressed)
        {
            hasResponse = true;
            isPressed = false;
            holdLvl = holdTime;
            onButtonHold?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData) => isPressed = true;
    public void OnPointerUp(PointerEventData eventData) => isPressed = false;

}
