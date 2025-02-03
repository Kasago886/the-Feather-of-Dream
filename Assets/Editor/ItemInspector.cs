using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Unity.VisualScripting;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(Item))]
public class ItemInspector : Editor
{
    Item item;
    int itemType;

    SerializedProperty dreamizedFeather;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        item = (Item)target;

        dreamizedFeather = serializedObject.FindProperty("dreamizedFeather");
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Undo.RecordObject(item,"Change Item");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        item.itemName = EditorGUILayout.TextField("物品名称",item.itemName);


        item.type = (ItemType)EditorGUILayout.EnumPopup("物品类型", item.type);
        if (item.type == ItemType.Feather || item.type == ItemType.BrokenFeather)
        {
            item.buffName = EditorGUILayout.TextField("buff名称",item.buffName);
        }
        if (item.type == ItemType.Feather)
        {
            item.featherHealth = EditorGUILayout.FloatField("羽当前生命值", item.featherHealth);
        }
        if (item.type == ItemType.BrokenFeather)
        {
            item.isDreamizable = EditorGUILayout.Toggle("可梦化", item.isDreamizable);
            if (item.isDreamizable)
            {
                item.dreamizeCost = EditorGUILayout.IntField("梦化消耗", item.dreamizeCost);

                EditorGUILayout.PropertyField(dreamizedFeather, new UnityEngine.GUIContent("梦羽"));
            }
        }

        EditorGUILayout.LabelField("详细信息");
        item.information = EditorGUILayout.TextArea(item.information);

        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(item);
        }

    }
}
