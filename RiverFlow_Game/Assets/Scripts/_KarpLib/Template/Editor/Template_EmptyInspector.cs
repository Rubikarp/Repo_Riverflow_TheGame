#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
//[CustomEditor(typeof(Template))]
public class Template_EmptyInspector : Editor
{

    // Start like
    private void OnEnable()
    {

    }

    // Update like
    public override void OnInspectorGUI()
    {

    }
}
#endif