using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(AttackBody))]
public class AttackBodyInspector : Editor
{
    AttackBody attackBody;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        attackBody = (AttackBody)target;
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(attackBody, "Change AttackBody");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        attackBody.isEnemy = EditorGUILayout.Toggle("是否是敌方替身",attackBody.isEnemy);
        attackBody.damage = EditorGUILayout.FloatField("伤害",attackBody.damage);

        attackBody.attackType = (AttackType)EditorGUILayout.EnumPopup("攻击类型", attackBody.attackType);
        if (attackBody.attackType == AttackType.Melee)
        {
            attackBody.attackCenter = EditorGUILayout.Vector2Field("攻击中心",attackBody.attackCenter);
            attackBody.attackRegion = EditorGUILayout.Vector2Field("攻击范围",attackBody.attackRegion);
        }

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(attackBody);

    }
}
