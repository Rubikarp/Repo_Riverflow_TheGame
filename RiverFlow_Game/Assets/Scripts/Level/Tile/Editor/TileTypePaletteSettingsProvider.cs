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
			TileTypePaletteSettingsProvider trsp = new TileTypePaletteSettingsProvider("_RiverFlow/TileTopologyPalette", SettingsScope.Project);
			trsp.guiHandler = OnProviderGUI;

			return trsp;
		}

		public static void OnProviderGUI(string context)
		{
			TileTopologyPalette trs = Resources.Load("TileTopologyPalette") as TileTopologyPalette;
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

		public static TileTopologyPalette CreateSettingsAsset()
		{
			var path = KarpToolUtilities.FindScriptFolder("TileTopologyPalette", true);
			if (!AssetDatabase.IsValidFolder(path + "Resources/"))
			{
				var folder = path.Remove(path.Length - 1);
				Debug.Log(folder);
				AssetDatabase.CreateFolder(folder, "Resources");
			}
			path += "Resources/TileTopologyPalette.asset";
			Debug.Log(path);
			var trs = ScriptableObject.CreateInstance<TileTopologyPalette>();
			AssetDatabase.CreateAsset(trs, path);
			//
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return trs;
		}
	}
}
