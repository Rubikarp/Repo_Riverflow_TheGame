using System;

public static class Extention_Time
{

    public static string ToFrenchString(this DateTime date)
    {
        return date.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));
    }
    public static string ToBritishString(this DateTime date)
    {
        return date.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-UK"));
    }
}
