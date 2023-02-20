using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Events;

public class MultipleResponse : MonoBehaviour
{
    public string[] responses;
    [SerializeField] private GameObject prefabButton;
    [SerializeField] private GameObject content;
    [SerializeField] private ScrollRect scrollView;
    private List<ButtonHold> responseButtons = new List<ButtonHold>();
    private List<TextMeshProUGUI> responseButtonsText = new List<TextMeshProUGUI>();

    public UnityEvent<string> onResponse;

    public void ShowResponses(string[] availableResponses)
    {
        responses = availableResponses;

        //Fill Array if necessary
        while (responses.Length > responseButtons.Count)
        {
            var button = Instantiate(prefabButton, content.transform).GetComponent<ButtonHold>();
            responseButtons.Add(button);
            responseButtonsText.Add(button.gameObject.GetComponentInChildren<TextMeshProUGUI>());
        }

        for (int i = 0; i < responseButtons.Count; i++)
        {
            responseButtons[i].gameObject.SetActive(i < responses.Length);
            responseButtonsText[i].text = i < responses.Length ? responses[i] : string.Empty;

            if (i < responses.Length)
            {
                //create a new reference for the lambda
                var index = i;
                responseButtons[i].onButtonHold.AddListener(() => ChooseRepsonse(index));
            }
        }
    }

    public void ScrollVertically(float value)
    {
        scrollView.velocity = Vector2.up * value;
    }

    public void ChooseRepsonse(int index)
    {
        onResponse?.Invoke(responses[index]);

        foreach (var responseButton in responseButtons)
        {
            responseButton.onButtonHold.RemoveAllListeners();
        }
        foreach (var buttonsText in responseButtonsText)
        {
            buttonsText.text = string.Empty;
        }
    }
}
