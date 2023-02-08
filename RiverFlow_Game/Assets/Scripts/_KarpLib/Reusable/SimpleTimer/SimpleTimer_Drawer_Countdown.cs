using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class SimpleTimer_Drawer_Countdown : MonoBehaviour
{
    [SerializeField] SimpleTimer timer;

    [SerializeField] string beforeText;
    [SerializeField] string afterText;
    [SerializeField] TextMeshProUGUI chronoText;

    public void Update()
    {
        if (chronoText is null) { } else chronoText.text = beforeText + string.Format("{0:0}", timer.TimeLeft) + afterText;
    }

}
