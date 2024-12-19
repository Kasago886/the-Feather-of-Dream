using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine.Events;

//CustomEditor用于关联要自定义的脚本
[CustomEditor(typeof(Item))]
public class ItemInspector : Editor
{
    Item item;
    int itemType;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        item = (Item)target;
    }

    //自定义Inspector面板
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(item,"Change Item");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        item.itemName = EditorGUILayout.TextField("物品名称",item.itemName);


        item.type = (ItemType)EditorGUILayout.EnumPopup("物品类型", item.type);
        if (item.type == ItemType.BrokenFeather)
        {
            item.isDreamizable = EditorGUILayout.Toggle("可梦化", item.isDreamizable);
            if (item.isDreamizable)
            {
                item.dreamizeCost = EditorGUILayout.IntField("梦化消耗", item.dreamizeCost);
            }
        }

        EditorGUILayout.LabelField("详细信息");
        item.information = EditorGUILayout.TextArea(item.information);

        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(item);

    }
}
