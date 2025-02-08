using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(Player),true)]
public class PlayerInspector : CharacterInspector
{
    Player player;

    //SerializedProperty cardGenerateTextProperty;
    SerializedProperty cardGenerateListProperty;
    SerializedProperty featherNumText;

    new protected void OnEnable()
    {
        base.OnEnable();
        //获取当前要自定义Inspector的对象
        player = (Player)target;

        //cardGenerateTextProperty = serializedObject.FindProperty("cardGenerateText");
        cardGenerateListProperty = serializedObject.FindProperty("cardGenerateList");
        featherNumText = serializedObject.FindProperty("featherNumText");
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(player, "Change Player");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        serializedObject.Update();
        //EditorGUILayout.PropertyField(cardGenerateTextProperty, new UnityEngine.GUIContent("cardGenerateText"));
        EditorGUILayout.PropertyField(featherNumText, new UnityEngine.GUIContent("featherNumText"));
        EditorGUILayout.PropertyField(cardGenerateListProperty, new UnityEngine.GUIContent("可以得到的卡牌"));
        serializedObject.ApplyModifiedProperties();

        player.cardGenerateCooldown = EditorGUILayout.FloatField("卡牌生成冷却时间",player.cardGenerateCooldown);

        EditorGUILayout.Space(10);

        EditorGUILayout.EndVertical();

        base.OnInspectorGUI();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(player);
        }
    }
}
