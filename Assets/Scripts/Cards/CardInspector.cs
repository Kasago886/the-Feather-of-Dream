using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Events;
[CustomEditor(typeof(Card), true)]
public class CardInspector:Editor
{
    private Card card;
    private bool b1, b2, b3;
    private SerializedProperty event1, event2, event3, event4,event5, buff, buffName;
    //作用对象
     bool effortOnPlayer;//作用于玩家
    bool effortOnEnmey;//作用于敌方
    bool effortOnOneEnemy;//作用于一个敌方
    bool effortOnMoreEnemies;//作用于多个敌方
    //public int theNumberOfEffortedEnemies;//作用的敌方个数
    ////作用方式
    bool click;
    bool dragOnCharactor;
    //public float minDistance;//最小距离
    //public UnityEvent whatHappenOnDrag;
    //public UnityEvent whatHappenWhenMouseEnter;
    //public UnityEvent whatHappenWhenMouseExit;
    //public UnityEvent effects;//卡牌效果
    //public Buff[] buffs;
    //public string[] buffNames;
    private void OnEnable()
    {
        card= (Card)target;
        effortOnPlayer = card.effortOnPlayer;effortOnEnmey = card.effortOnEnmey;effortOnOneEnemy = card.effortOnOneEnemy;effortOnMoreEnemies = card.effortOnMoreEnemies; click = card.click; dragOnCharactor = card.dragOnCharactor;
        event1 = serializedObject.FindProperty("whatHappenOnDrag");
        event2 = serializedObject.FindProperty("whatHappenWhenMouseEnter");
        event3 = serializedObject.FindProperty("whatHappenWhenMouseExit");
        event4 = serializedObject.FindProperty("effects");
        event5 = serializedObject.FindProperty("whatHappenWhenBeChoosen");
        buff = serializedObject.FindProperty("buffs");
        buffName= serializedObject.FindProperty("buffNames");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(2);

        b1=EditorGUILayout.Foldout( b1,"作用对象");
        if (b1)
        {
            card.effortOnPlayer = EditorGUILayout.Toggle("作用于玩家", card.effortOnPlayer);
            card.effortOnEnmey = EditorGUILayout.Toggle("作用于敌人", card.effortOnEnmey);
            if (card.effortOnPlayer != effortOnPlayer)//&&card.effortOnPlayer&& card.effortOnEnmey)
            {
                card.effortOnEnmey = false;
                effortOnPlayer = card.effortOnPlayer;
            }
            else if (card.effortOnEnmey != effortOnEnmey)//&& card.effortOnPlayer&&card.effortOnEnmey)
            {
                card.effortOnPlayer = false;
                effortOnEnmey = card.effortOnEnmey;
            }
            if (card.effortOnEnmey)
            {
                card.effortOnOneEnemy = EditorGUILayout.Toggle("作用于一个敌人", card.effortOnOneEnemy);
                card.effortOnMoreEnemies = EditorGUILayout.Toggle("作用于多个敌人", card.effortOnMoreEnemies);
                if (card.effortOnOneEnemy != effortOnOneEnemy)
                {
                    card.effortOnMoreEnemies = false;
                    card.theNumberOfEffortedEnemies = 0;
                    effortOnOneEnemy = card.effortOnOneEnemy;
                }
                else if (card.effortOnMoreEnemies != effortOnMoreEnemies)
                {
                    card.effortOnOneEnemy = false;
                    effortOnMoreEnemies = card.effortOnMoreEnemies;
                }
                if (card.effortOnMoreEnemies)
                {
                    card.theNumberOfEffortedEnemies = EditorGUILayout.IntField("被作用的敌方人数", card.theNumberOfEffortedEnemies);
                }
                if (card.theNumberOfEffortedEnemies > 0)
                {
                    EditorGUILayout.HelpBox("如果屏幕中的敌方数目少于所填数目，则被作用的敌方人数等于屏幕中的敌方数目", MessageType.Info);
                }
                else if (card.theNumberOfEffortedEnemies < 0)
                {
                    EditorGUILayout.HelpBox("不可以小于0", MessageType.Warning);
                    card.theNumberOfEffortedEnemies = 0;
                }
            }
        }
        EditorGUILayout.Space(2);

        b2 = EditorGUILayout.Foldout(b2, "作用方法");
        if (b2)
        {
            card.click = EditorGUILayout.Toggle("点击", card.click);
            if(card.click )
            {
                card.isRandom = EditorGUILayout.Toggle("主动选择", card.isRandom);
            }
            card.dragOnCharactor = EditorGUILayout.Toggle("拖拽", card.dragOnCharactor);
            if (card.dragOnCharactor)
            {
                card.minDistance = EditorGUILayout.FloatField("最小触发范围", card.minDistance);
                if (card.minDistance > 0)
                {
                    EditorGUILayout.HelpBox("注意实际范围", MessageType.Info);
                }
                else if (card.minDistance < 0)
                {
                    EditorGUILayout.HelpBox("不可以小于0", MessageType.Warning);
                    card.theNumberOfEffortedEnemies = 0;
                }
            }
        }
        EditorGUILayout.Space(2);

        b3 = EditorGUILayout.Foldout(b3, "作用效果");
        if (b3)
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("在卡牌被拖拽时所发生的事件");
            EditorGUILayout.PropertyField(event1);
            EditorGUILayout.LabelField("在鼠标在卡牌上所发生的事件");
            EditorGUILayout.PropertyField(event2);
            EditorGUILayout.LabelField("在鼠标离开卡牌所发生的事件");
            EditorGUILayout.PropertyField(event3);
            EditorGUILayout.LabelField("在卡牌发生作用时所发生的事件");
            EditorGUILayout.PropertyField(event4);
            EditorGUILayout.LabelField("在选择角色作为卡牌作用目标时所发生的事件");
            EditorGUILayout.PropertyField(event5);
            EditorGUILayout.PropertyField(buff,new GUIContent("需要添加的buff"),true);
            //for (int i = 0; i < buff.arraySize; i++)
            //{
            //    SerializedProperty element=buff.GetArrayElementAtIndex(i);
            //    EditorGUILayout.PropertyField (element,new GUIContent("Element"+i));
            //}
            EditorGUILayout.PropertyField(buffName, new GUIContent("需要添加的buff的名字"),true);
            //for (int i = 0; i < buffName.arraySize; i++)
            //{
            //    SerializedProperty element = buffName.GetArrayElementAtIndex(i);
            //    EditorGUILayout.PropertyField(element, new GUIContent("Element" + i));
            //}
            if (buff != null&&buffName!=null)
            {
                EditorGUILayout.HelpBox("注意:\"需要添加的buff\"与\"需要添加的buff的名字\"而这只需要填一个就行，如果这两上面个都填的是同一个buff那么将会对对象施加2次该buff", MessageType.Warning);
            }
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.EndVertical();
    }
}
