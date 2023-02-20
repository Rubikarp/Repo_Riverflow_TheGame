using UnityEngine;
//using UnityEngine.AddressableAssets;

public static class ProjectBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        //Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("#META#")));
    }

    // If using Addressable
    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ExecuteB()
    {
        Object.DontDestroyOnLoad(Addressables.InstantiateAsync("#META#").WaitForCompletion());
    }
    */
}

