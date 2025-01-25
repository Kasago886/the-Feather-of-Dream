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
    SerializedProperty bullet;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        attackBody = (AttackBody)target;

        bullet = serializedObject.FindProperty("bullet");
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(attackBody, "Change AttackBody");
        serializedObject.Update();

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        attackBody.attackType = (AttackType)EditorGUILayout.EnumPopup("攻击类型", attackBody.attackType);

        if (attackBody.attackType != AttackType.Child)
        {
            attackBody.isEnemy = EditorGUILayout.Toggle("是否是敌方替身", attackBody.isEnemy);
            attackBody.immediateAttack = EditorGUILayout.Toggle("是否立刻攻击", attackBody.immediateAttack);
            attackBody.damage = EditorGUILayout.FloatField("伤害", attackBody.damage);

            if (attackBody.attackType == AttackType.Melee)
            {
                attackBody.attackCenter = EditorGUILayout.Vector2Field("攻击中心", attackBody.attackCenter);
                attackBody.attackRegion = EditorGUILayout.Vector2Field("攻击范围", attackBody.attackRegion);
            }

            else if (attackBody.attackType == AttackType.Gun)
            {
                attackBody.attackCenter = EditorGUILayout.Vector2Field("子弹发射点", attackBody.attackCenter);
                EditorGUILayout.PropertyField(bullet, new GUIContent("子弹"));
                attackBody.isAiming = EditorGUILayout.Toggle("是否瞄准目标发射", attackBody.isAiming);
            }
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(attackBody);

    }
}
