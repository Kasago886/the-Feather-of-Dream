using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerInspector : Editor
{
    PlayerController playerController;

    bool showBottom;
    Vector2 bottomCenter;

    SerializedProperty attackEventProperty;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        playerController = (PlayerController)target;
        //获取attackEvent
        attackEventProperty = serializedObject.FindProperty("attackEvent");
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        //垂直方向布局
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("基础数值");
        playerController.walkSpeed = EditorGUILayout.FloatField("移动速度",playerController.walkSpeed);
        playerController.jumpSpeed = EditorGUILayout.FloatField("跳跃初速度",playerController.jumpSpeed);
        playerController.sprintSpeed = EditorGUILayout.FloatField("冲刺速度", playerController.sprintSpeed);
        playerController.sprintDuration = EditorGUILayout.FloatField("冲刺持续时间", playerController.sprintDuration);
        playerController.sprintCooldown = EditorGUILayout.FloatField("冲刺冷却时间", playerController.sprintCooldown);


        EditorGUILayout.LabelField("判定设置");
        showBottom = EditorGUILayout.Foldout(showBottom, "落地判定");
        if (showBottom)
        {
            bottomCenter = EditorGUILayout.Vector2Field("BottomCenter",new Vector2(playerController.bottomCenterX,playerController.bottomCenterY));
            playerController.bottomCenterX = bottomCenter.x;
            playerController.bottomCenterY = bottomCenter.y;

            playerController.bottomSize = EditorGUILayout.Vector2Field("BottomSize", playerController.bottomSize);
        }

        EditorGUILayout.LabelField("攻击事件");

        playerController.attackCooldown = EditorGUILayout.FloatField("攻击冷却时间", playerController.attackCooldown);

        serializedObject.Update();
        EditorGUILayout.PropertyField(attackEventProperty);

        EditorGUILayout.EndVertical();


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(playerController);
        }
    }
}
