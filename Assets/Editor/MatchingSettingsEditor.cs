using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StartMemoryMatching))]
[CanEditMultipleObjects]
[System.Serializable]

public class MatchingSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StartMemoryMatching myScript = target as StartMemoryMatching;

        if(myScript.CategoryButton == StartMemoryMatching.ECategoryButtontype.MatchingGameCategoryBtn)
        {
            myScript.memoryCategories = (MatchingSettings.EMatchingCategories)EditorGUILayout.EnumPopup("Categories", myScript.memoryCategories);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
