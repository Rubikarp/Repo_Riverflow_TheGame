#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
//using UnityEngine.AddressableAssets;

public static class ProjectBootstrapper
{
    public const string prefabName = "###BOOTSTRAPPER###";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        var prefab = Resources.Load(prefabName);
        if(prefab == null)
        {
            Debug.LogError($"No prefab named {prefabName} found in Resources folder");
            return;
        }
        var instance = Object.Instantiate(prefab);
        if (instance == null)
        {
            Debug.LogError($"Failed to instantiate prefab named {prefabName}");
            return;
        }
        Object.DontDestroyOnLoad(instance);
        Debug.Log($"[Bootstrapper] {instance.name} instantiated", instance);
    }

    // If using Addressable
    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ExecuteB()
    {
        Object.DontDestroyOnLoad(Addressables.InstantiateAsync(prefabName).WaitForCompletion());
    }
    */

#if UNITY_EDITOR
    [MenuItem("KarpProd/ShowBootstrap")]
    public static void OpenBootStrapProperties() => OpenPropertiesWindowOf(Resources.Load(prefabName));
    public static void OpenPropertiesWindowOf(Object pObject) => EditorUtility.OpenPropertyEditor(pObject);
#endif
}




