using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ResponseFreeHandler : MonoBehaviour
{
    public UnityEvent<string> onSendingResponse;
    public UnityEvent onResponseGiven;

    public Button button;
    public TMP_InputField inputField;

    private void OnEnable()
    {
        button?.onClick.AddListener(SendResponse);
    }
    private void OnDisable()
    {
        button?.onClick.RemoveListener(SendResponse);
        inputField.text = string.Empty;
    }

    public void SendResponse()
    {
        onSendingResponse?.Invoke(inputField.text);
        onResponseGiven?.Invoke();
        inputField.text = string.Empty;
    }
}
