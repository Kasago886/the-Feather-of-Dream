using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine.Events;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(Enemy),true)]
public class EnemyInspector : CharacterInspector
{
    Enemy enemy;
    int enemyType;

    new private void OnEnable()
    {
        base.OnEnable();

        //获取当前要自定义Inspector的对象
        enemy = (Enemy)target;
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(enemy, "Change Enemy");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("属性", EditorStyles.boldLabel);
        enemy.runSpeed = EditorGUILayout.FloatField("移动速度",enemy.runSpeed);
        enemy.jumpSpeed = EditorGUILayout.FloatField("跳跃初速度",enemy.jumpSpeed);
        enemy.attackCooldown = EditorGUILayout.FloatField("攻击冷却", enemy.attackCooldown);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("索敌",EditorStyles.boldLabel);
        enemy.searchType = (EnemySearchType)EditorGUILayout.EnumPopup("索敌类型", enemy.searchType);

        if ( enemy.searchType == EnemySearchType.distance || enemy.searchType == EnemySearchType.horizontal)
        {
            enemy.searchDistance = EditorGUILayout.FloatField("索敌距离", enemy.searchDistance);
        }

        enemy.wallDetect = EditorGUILayout.Toggle("墙体检测",enemy.wallDetect);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("攻击卡");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("使用距离", GUILayout.Width(50));
        enemy.attackCardUseDistance = EditorGUILayout.FloatField(enemy.attackCardUseDistance);
        EditorGUILayout.LabelField("冷却时间", GUILayout.Width(50));
        enemy.attackCardCooldown = EditorGUILayout.FloatField(enemy.attackCardUseDistance);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("效果卡");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("使用距离", GUILayout.Width(50));
        enemy.effectCardUseDistance = EditorGUILayout.FloatField(enemy.effectCardUseDistance);
        EditorGUILayout.LabelField("冷却时间", GUILayout.Width(50));
        enemy.effectCardCooldown = EditorGUILayout.FloatField(enemy.effectCardCooldown);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(enemy);

        base.OnInspectorGUI();
    }
}
