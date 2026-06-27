using System;
using UnityEngine;

public static class TextureStorage
{
    // ─── Save ────────────────────────────────────────────────────────────────
    public static void Save(this Texture2D tex, RenderTexture source)
    {
        if (!ValidateSource(source, nameof(tex))) return;

        Texture2D baked = BakeToTexture2D(source);

        tex.Reinitialize(baked.width, baked.height, baked.format, baked.mipmapCount > 1);
        tex.SetPixels(baked.GetPixels());
        tex.Apply();

        byte[] png = baked.EncodeToPNG();
        UnityEngine.Object.Destroy(baked);

        OverwriteAsset(tex, png);
        Persist(tex.name, png);
    }
    public static void Save(RenderTexture source, string key)
    {
        if (!ValidateSource(source, key)) return;

        Texture2D baked = BakeToTexture2D(source);
        byte[] png = baked.EncodeToPNG();
        UnityEngine.Object.Destroy(baked);

        Persist(key, png);
    }
    public static void Save(Texture2D tex, string key)
    {
        if (!ValidateTexture(tex, key)) return;

        Persist(key, tex.EncodeToPNG());
    }
    
    // ─── Load ────────────────────────────────────────────────────────────────
    public static Texture2D Load(string key)
    {
        byte[] png = ReadBytes(key);
        if (png == null)
        {
            Debug.LogWarning($"[TextureStorage] '{key}' not found, initiate with default texture");
            return new Texture2D(256, 256, TextureFormat.RGBA32, false);
        }

        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(png);
        tex.name = key;
        return tex;
    }

    // ─── Delete ──────────────────────────────────────────────────────────────
    public static void Delete(string key)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.DeleteKey(BuildPrefsKey(key));
        PlayerPrefs.Save();
#else
        string path = BuildFilePath(key);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
#endif
    }

    public static Texture2D BakeToTexture2D(this RenderTexture rt)
    {
        var previous = RenderTexture.active;
        RenderTexture.active = rt;

        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = previous;
        return tex;
    }

    // ───— Validation ─────────────────────────────────────────────────
    private static bool ValidateSource(RenderTexture source, string key)
    {
        if (source != null) return true;
        Debug.LogError($"[TextureStorage] Cannot save null RenderTexture (key: '{key}')");
        return false;
    }
    private static bool ValidateTexture(Texture2D tex, string key)
    {
        if (tex != null) return true;
        Debug.LogError($"[TextureStorage] Cannot save null Texture2D (key: '{key}')");
        return false;
    }

    // ─── Private — Persistence ────────────────────────────────────────────────
    private static void Persist(string key, byte[] png)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.SetString(BuildPrefsKey(key), Convert.ToBase64String(png));
        PlayerPrefs.Save();
#else
        System.IO.File.WriteAllBytes(BuildFilePath(key), png);
#endif
        Debug.Log($"[TextureStorage] Saved '{key}' ({png.Length} bytes)");
    }

    private static byte[] ReadBytes(string key)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        string raw = PlayerPrefs.GetString(BuildPrefsKey(key), null);
        if (string.IsNullOrEmpty(raw)) return null;
        return Convert.FromBase64String(raw);
#else
        string path = BuildFilePath(key);
        if (!System.IO.File.Exists(path)) return null;
        return System.IO.File.ReadAllBytes(path);
#endif
    }

    private static void OverwriteAsset(Texture2D tex, byte[] png)
    {
#if UNITY_EDITOR
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(tex);
        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogWarning($"[TextureStorage] '{tex.name}' n'est pas un asset de projet.");
            return;
        }

        System.IO.File.WriteAllBytes(assetPath, png);
        UnityEditor.AssetDatabase.ImportAsset(assetPath, UnityEditor.ImportAssetOptions.ForceUpdate);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"[TextureStorage] Asset overwritten: {assetPath}");
#endif
    }
    
    // ─── Private — Path Builders ──────────────────────────────────────────────
    private static string BuildFilePath(string key) => System.IO.Path.Combine(Application.persistentDataPath, $"{key}.png");
    private static string BuildPrefsKey(string key) => $"rts_{key}";
}