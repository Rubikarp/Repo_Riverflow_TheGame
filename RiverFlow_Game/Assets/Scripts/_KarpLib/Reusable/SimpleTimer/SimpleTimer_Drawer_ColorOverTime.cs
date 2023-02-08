using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimpleTimer_Drawer_ColorOverTime : MonoBehaviour
{
    [SerializeField] SimpleTimer timer;
    [Space(10)]
    public Image[] images;

    public ColorTime[] paliers;


    private void Update()
    {
        var order = paliers.OrderByDescending(x => x.time).ToArray();
        for (int i = 0; i < paliers.Length; i++)
        {
            if (timer.TimeLeft < order[i].time) continue;

            foreach (var image in images)
            {
                image.color = order[i].color;
            }
            return;
        }
    }
}

[System.Serializable]
public class ColorTime
{
    public float time;
    public Color color;
}