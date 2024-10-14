using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class ColorExtend
{
    public static Color GetColor(this SpriteRenderer obj)
    {
        return obj.color;
    }

    public static Color SetColor(this SpriteRenderer obj, Color color)
    {
        return obj.color = color;
    }

    public static Color SetAlpha(this SpriteRenderer obj, float alpha)
    {
        Color color = obj.color;
        color.a = alpha;
        obj.color = color;
        return color;
    }

    public static Color GetColor(this Graphic obj)
    {
        return obj.color;
    }

    public static Color SetColor(this Graphic obj, Color color)
    {
        return obj.color = color;
    }

    public static Color SetAlpha(this Graphic obj, float alpha)
    {
        Color color = obj.color;
        color.a = alpha;
        obj.color = color;
        return color;
    }

    public static Color GetColor(this Material obj)
    {
        return obj.color;
    }

    public static Color SetColor(this Material obj, Color color)
    {
        return obj.color = color;
    }

    public static Color SetAlpha(this Material obj, float alpha)
    {
        Color color = obj.color;
        color.a = alpha;
        obj.color = color;
        return color;
    }

    public static Color SetColor(this MeshRenderer obj, Color color)
    {
        obj.sharedMaterial.color = color;
        return color;
    }

    public static Color DOColor(this MeshRenderer obj, Gradient gradient, float duration = 1f, float from = 0f, float to = 1f)
    {
        _ = obj.material;
        Material sharedMaterial = obj.sharedMaterial;
        Color color = gradient.Evaluate(from);
        sharedMaterial.color = color;
        DOVirtual.Float(from, to, duration, delegate (float s)
        {
            sharedMaterial.color = (color = gradient.Evaluate(s));
        });
        return color;
    }

    public static Color SetAlpha(this MeshRenderer obj, float alpha)
    {
        _ = obj.material;
        Material sharedMaterial = obj.sharedMaterial;
        Color color = sharedMaterial.color;
        color.a = alpha;
        sharedMaterial.color = color;
        return color;
    }

    public static string ConvertStringColor(string str, Color color)
    {
        return "<color=#" + ConverterColor(color) + ">" + str + "</color>";
    }

    public static string ConverterColor(Color color, int alpha = 255)
    {
        string result = "#BE0A00";
        try
        {
            result = ColorUtility.ToHtmlStringRGBA(new Color(color.r, color.g, color.b, (float)alpha / 255f));
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError("RGBConverter: " + ex.Message + "\n" + ex.StackTrace);
            return result;
        }
    }

    public static Color SetAlpha(Color color, int alpha = -1)
    {
        color.a = ((alpha >= 0) ? ((float)alpha / 255f) : 1f);
        return color;
    }

    public static Color SetAlpha(Color color, float alpha = -1f)
    {
        color.a = ((alpha >= 0f) ? alpha : 1f);
        return color;
    }
}