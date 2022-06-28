using Karprod;
using UnityEditor;
using UnityEngine;

namespace RiverFlow.Core
{
    public class TileTypePaletteSettingsProvider : SettingsProvider
	{
		public TileTypePaletteSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		static Editor editor;

		[SettingsProvider]
		public static SettingsProvider CreateProviderForProjectSettings()
		{
			TileTypePaletteSettingsProvider trsp = new TileTypePaletteSettingsProvider("_RiverFlow/TileTypePalette", SettingsScope.Project);
			trsp.guiHandler = OnProviderGUI;

			return trsp;
		}

		public static void OnProviderGUI(string context)
		{
			TileTypePalette trs = Resources.Load("TileTypePalette") as TileTypePalette;
			if (trs is null)
			{
				trs = CreateSettingsAsset();
			}
			if (!editor)
			{
				Editor.CreateCachedEditor(trs, null, ref editor);
			}
			editor.OnInspectorGUI();
		}

		public static TileTypePalette CreateSettingsAsset()
		{
			var path = KarpToolUtilities.FindScriptFolder("TileTypePalette", true);
			if (!AssetDatabase.IsValidFolder(path + "Resources/"))
			{
				var folder = path.Remove(path.Length - 1);
				Debug.Log(folder);
				AssetDatabase.CreateFolder(folder, "Resources");
			}
			path += "Resources/TileTypePalette.asset";
			Debug.Log(path);
			var trs = ScriptableObject.CreateInstance<TileTypePalette>();
			AssetDatabase.CreateAsset(trs, path);
			//
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return trs;
		}
	}
}
