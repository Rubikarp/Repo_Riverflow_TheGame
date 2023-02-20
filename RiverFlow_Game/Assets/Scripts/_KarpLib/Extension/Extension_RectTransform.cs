using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Extension_RectTransform
{
    public static void SetTop(this RectTransform rt, float top) => rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    public static void SetLeft(this RectTransform rt, float left) => rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    public static void SetRight(this RectTransform rt, float right) => rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    public static void SetBottom(this RectTransform rt, float bottom) => rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);

    public static void AddOffsetHori(this RectTransform rt, float value)
    {
        rt.offsetMin = new Vector2(rt.offsetMax.x + value, rt.offsetMin.y);
        rt.offsetMax = new Vector2(rt.offsetMax.x + value, rt.offsetMax.y);
    }
    public static void AddOffsetVert(this RectTransform rt, float value)
    {
        rt.offsetMin = new Vector2(rt.offsetMax.x, rt.offsetMin.y + value);
        rt.offsetMax = new Vector2(rt.offsetMax.x, rt.offsetMax.y + value);
    }

    public static T ChangeAlpha<T>(this T g, float alpha) where T : Graphic
    {
        g.color = new Color(g.color.r, g.color.g, g.color.b, alpha);
        return g;
    }

}
