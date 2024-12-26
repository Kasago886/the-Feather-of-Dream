using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    Character character;

    private void OnEnable()
    {
        //获取当前要自定义Inspector的对象
        character = (Character)target;
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(character, "Change Character");

        //垂直方向布局
        EditorGUILayout.BeginVertical();

        character.isDefaultFeather = EditorGUILayout.ToggleLeft("使用默认羽", character.isDefaultFeather);
        if (character.isDefaultFeather)
        {
            character.defaultFeatherNum = EditorGUILayout.IntField("默认羽数",character.defaultFeatherNum);

            character.feathers = new();
            for (int i = 0; i < character.defaultFeatherNum; i++)
            {
                character.feathers.Add(new DefautFeather());
            }
        }

        character.tenacity = EditorGUILayout.FloatField("韧性",character.tenacity);
        character.strength = EditorGUILayout.FloatField("力量", character.strength);


        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(character);
    }
}
