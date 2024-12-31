using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(Character),true)]
public class CharacterInspector : Editor
{
    Character character;
    bool setDefaultTS;

    SerializedProperty injuryEventProperty;
    SerializedProperty healEventProperty;
    SerializedProperty deathEventProperty;

    protected void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        character = (Character)target;

        //获取property
        injuryEventProperty = serializedObject.FindProperty("injuryEvent");
        healEventProperty = serializedObject.FindProperty("healEvent");
        deathEventProperty = serializedObject.FindProperty("deathEvent");
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(character, "Change Character");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        character.isDefaultFeather = EditorGUILayout.Toggle("添加初始羽", character.isDefaultFeather);
        if (character.isDefaultFeather)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("初始羽数", GUILayout.Width(50));
            character.defaultFeatherNum = EditorGUILayout.IntField(character.defaultFeatherNum);
            if (character.defaultFeatherNum < 0)
                character.defaultFeatherNum = 0;//防止小于0

            EditorGUILayout.LabelField("初始羽血量", GUILayout.Width(65));
            character.defaultFeatherHealth = EditorGUILayout.FloatField(character.defaultFeatherHealth);

            EditorGUILayout.EndHorizontal();
        }

        setDefaultTS = EditorGUILayout.BeginFoldoutHeaderGroup(setDefaultTS, "设置初始数值（对玩家无效）");
        if (setDefaultTS)
        {
            character.tenacity = EditorGUILayout.FloatField("韧性", character.tenacity);
            character.strength = EditorGUILayout.FloatField("力量", character.strength);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(10);

        serializedObject.Update();
        EditorGUILayout.PropertyField(injuryEventProperty, new UnityEngine.GUIContent("受伤事件"));
        EditorGUILayout.PropertyField(healEventProperty, new UnityEngine.GUIContent("治疗事件"));
        EditorGUILayout.PropertyField(deathEventProperty, new UnityEngine.GUIContent("死亡事件"));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(character);
    }
}
