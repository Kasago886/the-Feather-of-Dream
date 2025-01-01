using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
[CustomEditor(typeof(Card), true)]
public class CardInspector:Editor
{
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
    }
}
