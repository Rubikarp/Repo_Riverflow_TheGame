using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BinaryQuestion : MonoBehaviour
{
    public TMP_Animated questionSlot;

    public Button buttonYes;
    public TextMeshProUGUI textSlotYes;

    public Button buttonNo;
    public TextMeshProUGUI textSlotNo;

    private void OnEnable()
    {
        buttonNo.interactable = false;
        buttonYes.interactable = false;
    }
    private void Start()
    {
        questionSlot.onDialogueEnd.AddListener(() =>
        {
            buttonNo.interactable = true;
            buttonYes.interactable = true;
        });
    }

    private bool? response = null;
    public async Task<bool> AskBinaryQuestion(string question, string answerYes,string answerNo)
    {
        response = null;
        questionSlot.text = question;
        questionSlot.ReadText();

        textSlotNo.text = answerNo;
        textSlotYes.text = answerYes;

        buttonNo.onClick.AddListener(()=> response = false);
        buttonYes.onClick.AddListener(() => response = true);

        while (response is null)
        {
            await Task.Yield();
        }

        return (bool)response;
    }
}
