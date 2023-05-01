using Karprod;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    public class RiverPaletteSettingsProvider : SettingsProvider
	{
		public RiverPaletteSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		static Editor editor;

		[SettingsProvider]
		public static SettingsProvider CreateProviderForProjectSettings()
		{
            RiverPaletteSettingsProvider trsp = new RiverPaletteSettingsProvider("_RiverFlow/RiverPalette", SettingsScope.Project);
			trsp.guiHandler = OnProviderGUI;

			return trsp;
		}

		public static void OnProviderGUI(string context)
		{
            RiverPalette trs = Resources.Load("RiverPalette") as RiverPalette;
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

		public static RiverPalette CreateSettingsAsset()
		{
			var path = KarpToolUtilities.FindScriptFolder("RiverPalette", true);
			if (!AssetDatabase.IsValidFolder(path + "Resources/"))
			{
				var folder = path.Remove(path.Length - 1);
				Debug.Log(folder);
				AssetDatabase.CreateFolder(folder, "Resources");
			}
			path += "Resources/RiverPalette.asset";
			Debug.Log(path);
			var rms = ScriptableObject.CreateInstance<RiverPalette>();
			AssetDatabase.CreateAsset(rms, path);
			//
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return rms;
		}

	}
}
