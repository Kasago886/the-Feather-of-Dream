using Cinemachine.Editor;
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

    SerializedProperty injuryEvent;
    SerializedProperty healEvent;
    SerializedProperty deathEvent;
    SerializedProperty hpScroll;

    SerializedProperty attackSound;
    SerializedProperty injurySound;
    SerializedProperty deathSound;

    protected void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        character = (Character)target;

        //获取property
        injuryEvent = serializedObject.FindProperty("injuryEvent");
        healEvent = serializedObject.FindProperty("healEvent");
        deathEvent = serializedObject.FindProperty("deathEvent");
        hpScroll = serializedObject.FindProperty("hpScroll");
        attackSound = serializedObject.FindProperty("attackSound");
        injurySound = serializedObject.FindProperty("injurySound");
        deathSound = serializedObject.FindProperty("deathSound");
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(character, "Change Character");
        serializedObject.Update();

        //垂直方向布局
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(hpScroll, new UnityEngine.GUIContent("血条ui ScrollView"));

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
            character.abnormalityResistance = EditorGUILayout.FloatField("异常抗性", character.abnormalityResistance);
            character.burnResistance = EditorGUILayout.FloatField("灼伤抵抗", character.burnResistance);
            character.traumaResistance= EditorGUILayout.FloatField("伤痕抵抗", character.traumaResistance);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(10);

        character.injuryForceback = EditorGUILayout.Toggle("受击击退（对玩家无效）", character.injuryForceback);
        if (character.injuryForceback)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("击退力", GUILayout.Width(50));
            character.forcebackForce = EditorGUILayout.FloatField(character.forcebackForce);
            EditorGUILayout.LabelField("击退时间", GUILayout.Width(50));
            character.forcebackDuration = EditorGUILayout.FloatField(character.forcebackDuration);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(attackSound, new UnityEngine.GUIContent("攻击音效"));
        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(injurySound, new UnityEngine.GUIContent("受伤音效"));
        EditorGUILayout.PropertyField(injuryEvent, new UnityEngine.GUIContent("受伤事件"));

        EditorGUILayout.PropertyField(healEvent, new UnityEngine.GUIContent("治疗事件"));

        EditorGUILayout.PropertyField(deathSound, new UnityEngine.GUIContent("死亡音效"));
        EditorGUILayout.PropertyField(deathEvent, new UnityEngine.GUIContent("死亡事件"));

        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(character);
        }
    }
}
