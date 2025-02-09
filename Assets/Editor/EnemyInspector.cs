using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(Enemy),true)]
public class EnemyInspector : CharacterInspector
{
    Enemy enemy;
    int enemyType;

    SerializedProperty attackCardLineList;
    SerializedProperty effectCardLineList;
    SerializedProperty dropItems;

    new private void OnEnable()
    {
        base.OnEnable();

        //获取当前要自定义Inspector的对象
        enemy = (Enemy)target;

        attackCardLineList = serializedObject.FindProperty("attackCardLineList");
        effectCardLineList = serializedObject.FindProperty("effectCardLineList");
        dropItems = serializedObject.FindProperty("dropItems");
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(enemy, "Change Enemy");
        serializedObject.Update();

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("属性", EditorStyles.boldLabel);
        enemy.enemyName = EditorGUILayout.TextField("名字",enemy.enemyName);
        enemy.runSpeed = EditorGUILayout.FloatField("移动速度",enemy.runSpeed);
        enemy.jumpSpeed = EditorGUILayout.FloatField("跳跃初速度",enemy.jumpSpeed);
        enemy.attackCooldown = EditorGUILayout.FloatField("攻击冷却", enemy.attackCooldown);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("索敌",EditorStyles.boldLabel);
        enemy.keepDistanceWhenNotArmed = EditorGUILayout.Toggle("无攻击替身时保持距离", enemy.keepDistanceWhenNotArmed);
        enemy.searchType = (EnemySearchType)EditorGUILayout.EnumPopup("索敌类型", enemy.searchType);

        if ( enemy.searchType == EnemySearchType.distance || enemy.searchType == EnemySearchType.horizontal)
        {
            enemy.searchDistance = EditorGUILayout.FloatField("索敌最远距离", enemy.searchDistance);
        }
        if (enemy.keepDistanceWhenNotArmed)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("无攻击替身时\n保持的距离", GUILayout.Width(120), GUILayout.Height(30));
            enemy.minDistance = EditorGUILayout.FloatField(enemy.minDistance,GUILayout.Height(30));
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            enemy.minDistance = EditorGUILayout.FloatField("追击时的最近距离", enemy.minDistance);
        }

        enemy.wallDetect = EditorGUILayout.Toggle("墙体检测",enemy.wallDetect);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("攻击卡");
        EditorGUILayout.LabelField("使用距离", GUILayout.Width(50));
        enemy.attackCardUseDistance = EditorGUILayout.FloatField(enemy.attackCardUseDistance);
        EditorGUILayout.PropertyField(attackCardLineList, new UnityEngine.GUIContent("攻击卡"), true);

        EditorGUILayout.LabelField("效果卡");
        EditorGUILayout.LabelField("使用距离", GUILayout.Width(50));
        enemy.effectCardUseDistance = EditorGUILayout.FloatField(enemy.effectCardUseDistance);
        EditorGUILayout.PropertyField(effectCardLineList, new UnityEngine.GUIContent("效果卡"), true);

        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(dropItems, new UnityEngine.GUIContent("掉落物"), true);
        enemy.exp = EditorGUILayout.IntField("死亡获得经验值",enemy.exp);

        EditorGUILayout.Space(10);

        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(enemy);
        }

        base.OnInspectorGUI();
    }
}
