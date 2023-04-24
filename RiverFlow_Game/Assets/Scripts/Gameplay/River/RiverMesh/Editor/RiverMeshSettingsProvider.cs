using Karprod;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    public class RiverMeshSettingsProvider : SettingsProvider
	{
		public RiverMeshSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		static Editor editor;

		[SettingsProvider]
		public static SettingsProvider CreateProviderForProjectSettings()
		{
			RiverMeshSettingsProvider trsp = new RiverMeshSettingsProvider("_RiverFlow/RiverMeshSettings", SettingsScope.Project);
			trsp.guiHandler = OnProviderGUI;

			return trsp;
		}

		public static void OnProviderGUI(string context)
		{
			RiverMeshSettings trs = Resources.Load("RiverMeshSettings") as RiverMeshSettings;
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

		public static RiverMeshSettings CreateSettingsAsset()
		{
			var path = KarpToolUtilities.FindScriptFolder("RiverMeshSettings", true);
			if (!AssetDatabase.IsValidFolder(path + "Resources/"))
			{
				var folder = path.Remove(path.Length - 1);
				Debug.Log(folder);
				AssetDatabase.CreateFolder(folder, "Resources");
			}
			path += "Resources/RiverMeshSettings.asset";
			Debug.Log(path);
			var rms = ScriptableObject.CreateInstance<RiverMeshSettings>();
			AssetDatabase.CreateAsset(rms, path);
			//
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return rms;
		}

	}
}
