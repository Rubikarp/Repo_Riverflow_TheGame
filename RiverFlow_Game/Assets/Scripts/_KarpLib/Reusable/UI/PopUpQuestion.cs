using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PopUpQuestion : MonoBehaviour
{
    public BinaryQuestion popUp;

    public async Task<bool> AskBinaryQuestion(string question, string answerYes, string answerNo)
    {
        popUp.gameObject.SetActive(true);
        bool answer = await popUp.AskBinaryQuestion(question, answerYes, answerNo);
        popUp.gameObject.SetActive(false);

        return answer;
    }
}
