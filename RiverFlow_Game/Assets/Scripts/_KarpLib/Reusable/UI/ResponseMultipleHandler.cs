using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResponseMultipleHandler : MonoBehaviour
{
    [SerializeField] Button[] answerButtons = new Button[5];
    [Header("Responses")]
    [SerializeField] string[] possibleTextResponse = new string[5];
    [SerializeField] Texture[] possibleImgResponse = new Texture[5];
    [Header("Slot")]
    [SerializeField] TextMeshProUGUI[] responseTextSlot = new TextMeshProUGUI[5];
    [SerializeField] RawImage[] responseImageSlot = new RawImage[5];

    public UnityEvent<string> onSendingResponse;
    public UnityEvent onResponseGiven;

    public int ResponseGiven { get; private set; }

    [NaughtyAttributes.Button]
    public void TestSetUpTextResponse() => SetUpResponse(possibleTextResponse);
    [NaughtyAttributes.Button]
    public void TestSetUpImageResponse() => SetUpResponse(possibleImgResponse);

    private void Start()
    {
        ResetResponseGiven();
    }

    public void ResetResponseGiven() => ResponseGiven = -1;

    public void SetUpResponse(string[] response)
    {
        SetActiveTextSlot(true);
        SetActiveImgSlot(false);
        possibleTextResponse = response;

        bool isVisible;
        for (int i = 0; i < 5; i++)
        {
            isVisible = i < response.Length;
            answerButtons[i].gameObject.SetActive(isVisible);
            responseTextSlot[i].text = isVisible ? possibleTextResponse[i] : string.Empty;
        }
    }
    public void SetUpResponse(Texture[] response)
    {
        SetActiveTextSlot(false);
        SetActiveImgSlot(true);
        possibleImgResponse = response;

        bool isVisible;
        for (int i = 0; i < 5; i++)
        {
            isVisible = i < response.Length;
            answerButtons[i].gameObject.SetActive(isVisible);
            responseImageSlot[i].texture = isVisible ? possibleImgResponse[i] : Texture2D.whiteTexture;
        }
    }

    public void DisableResponse()
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }
        SetActiveTextSlot(false);
        SetActiveImgSlot(false);
    }
    private void SetActiveTextSlot(bool visible)
    {
        foreach (var slot in responseTextSlot)
        {
            slot.gameObject.SetActive(visible);
            slot.text = visible? slot.text : string.Empty;
        }
    }
    private void SetActiveImgSlot(bool visible)
    {
        foreach (var slot in responseImageSlot)
        {
            slot.gameObject.SetActive(visible);
            slot.texture = visible ? slot.texture : Texture2D.whiteTexture;
        }
    }

    public void ResponseToSend(int repNbr)
    {
        ResponseGiven = repNbr;

        //Text Response
        if (responseTextSlot[0].IsActive()) 
            onSendingResponse?.Invoke(responseTextSlot[repNbr].text);
        //Icon response
        else onSendingResponse?.Invoke(repNbr.ToString());

        onResponseGiven?.Invoke();
    }
}
