using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SimpleTimer_Drawer_ImageFill : MonoBehaviour
{
    [SerializeField] SimpleTimer timer;
    [SerializeField] Image image;

    public void Update()
    {
        image.fillAmount = timer.TimeLeft / timer.duration;
    }

}
