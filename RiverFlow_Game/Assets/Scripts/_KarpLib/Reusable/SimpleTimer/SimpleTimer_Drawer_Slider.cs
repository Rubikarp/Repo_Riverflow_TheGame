using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SimpleTimer_Drawer_Slider : MonoBehaviour
{
    [SerializeField] SimpleTimer timer;

    [SerializeField] Slider slider;

    public void Update()
    {
        slider.maxValue = timer.duration;
        slider.value = timer.TimeLeft;
    }

}
