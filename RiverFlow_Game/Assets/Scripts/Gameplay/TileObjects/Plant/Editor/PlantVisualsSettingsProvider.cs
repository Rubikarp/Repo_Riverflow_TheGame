using Karprod;
using UnityEditor;
using UnityEngine;

namespace RiverFlow.Core
{
    public class PlantVisualDataSettingsProvider : SettingsProvider
    {
        public PlantVisualDataSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

        static Editor editor;

        [SettingsProvider]
        public static SettingsProvider CreateProviderForProjectSettings()
        {
            PlantVisualDataSettingsProvider trsp = new PlantVisualDataSettingsProvider("_RiverFlow/PlantVisualData", SettingsScope.Project);
            trsp.guiHandler = OnProviderGUI;

            return trsp;
        }

        public static void OnProviderGUI(string context)
        {
            PlantVisualData trs = Resources.Load("PlantVisualData") as PlantVisualData;
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

        public static PlantVisualData CreateSettingsAsset()
        {
            var path = KarpToolUtilities.FindScriptFolder("PlantVisualData", true);
            if (!AssetDatabase.IsValidFolder(path + "Resources/"))
            {
                var folder = path.Remove(path.Length - 1);
                Debug.Log(folder);
                AssetDatabase.CreateFolder(folder, "Resources");
            }
            path += "Resources/PlantVisualData.asset";
            Debug.Log(path);
            var trs = ScriptableObject.CreateInstance<PlantVisualData>();
            AssetDatabase.CreateAsset(trs, path);
            //
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return trs;
        }
    }

}
