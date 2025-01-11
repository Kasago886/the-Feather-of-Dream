using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(Player),true)]
public class PlayerInspector : CharacterInspector
{
    Player player;

    SerializedProperty hpScrollProperty;

    new protected void OnEnable()
    {
        base.OnEnable();
        //获取当前要自定义Inspector的对象
        player = (Player)target;

        hpScrollProperty = serializedObject.FindProperty("hpScroll");
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(player, "Change Player");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        serializedObject.Update();
        EditorGUILayout.PropertyField(hpScrollProperty, new UnityEngine.GUIContent("血条ui ScrollView"));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();

        base.OnInspectorGUI();

        EditorUtility.SetDirty(player);
    }
}
