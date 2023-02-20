using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class SimpleTimer_Drawer_Chrono : MonoBehaviour
{
    [SerializeField] private SimpleTimer timer;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI textSlotMinutes;
    [SerializeField] private TextMeshProUGUI textSlotSeconds;

    public void Update()
    {
        if (textSlotSeconds is null) { } else textSlotSeconds.text = string.Format("{0:00}", Mathf.FloorToInt(timer.TimeLeft % 60));
        if (textSlotMinutes is null) { } else textSlotMinutes.text = string.Format("{0:00}", Mathf.FloorToInt(timer.TimeLeft / 60));
    }
}
