using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent simpleClick;
    public UnityEvent doubleClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.clickCount)
        {
            case 1: simpleClick?.Invoke();
                break;
            case 2: doubleClick?.Invoke();
                break;
        }
    }
}
