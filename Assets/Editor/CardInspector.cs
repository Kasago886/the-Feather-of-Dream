using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UI;
[CustomEditor(typeof(Card), true)]
public class CardInspector : Editor
{

    private Card card;
    private bool b0, b1, b2, b3, b4;
    private SerializedProperty event1, event2, event3, event4, event5, buff, buffName, event6, event7, event8, event9, event10, buff1, buffName1, event11, event12, event13, event14, event15, buff2, buffName2;
    private GameObject prefob;
    bool effortOnPlayer;//作用于玩家
    bool effortOnEnmey;//作用于敌方
    bool effortOnOneEnemy;//作用于一个敌方
    bool effortOnMoreEnemies;//作用于多个敌方
    bool click;
    bool dragOnCharactor;
    bool playerUse;
    bool enemyUse;
    private void OnEnable()
    {
        b1 = true; b2 = true; b0 = true; b4 = true;
        card = (Card)target;
        effortOnPlayer = card.effortOnPlayer; effortOnEnmey = card.effortOnEnmey; effortOnOneEnemy = card.effortOnOneEnemy; effortOnMoreEnemies = card.effortOnMoreEnemies; click = card.click; dragOnCharactor = card.dragOnCharactor;
        event1 = serializedObject.FindProperty("whatHappenOnDrag");
        event2 = serializedObject.FindProperty("whatHappenWhenMouseEnter");
        event3 = serializedObject.FindProperty("whatHappenWhenMouseExit");
        event4 = serializedObject.FindProperty("effects");
        event5 = serializedObject.FindProperty("whatHappenWhenBeChoosen");
        event6 = serializedObject.FindProperty("whatHappenOnDragPlayer");
        event7 = serializedObject.FindProperty("whatHappenWhenMouseEnterPlayer");
        event8 = serializedObject.FindProperty("whatHappenWhenMouseExitPlayer");
        event9 = serializedObject.FindProperty("effectsPlayer");
        event10 = serializedObject.FindProperty("whatHappenWhenBeChoosenEnemy");
        event11 = serializedObject.FindProperty("whatHappenOnDragEnemy");
        event12 = serializedObject.FindProperty("whatHappenWhenMouseEnterEnemy");
        event13 = serializedObject.FindProperty("whatHappenWhenMouseExitEnemy");
        event14 = serializedObject.FindProperty("effectsEnemy");
        event15 = serializedObject.FindProperty("whatHappenWhenBeChoosenEnemy");
        buff = serializedObject.FindProperty("buffs");
        buffName = serializedObject.FindProperty("buffNames");
        buff1 = serializedObject.FindProperty("buffsPlayer");
        buffName1 = serializedObject.FindProperty("buffNamesPlayer");
        buff2 = serializedObject.FindProperty("buffsEnemy");
        buffName2 = serializedObject.FindProperty("buffNamesEnemy");
        card.playerUse = true;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(2);
        b4 = EditorGUILayout.Foldout(b4, "使用者");
        if (b4)
        {
            card.playerUse = EditorGUILayout.Toggle("玩家使用", card.playerUse);
            card.enemyUse = EditorGUILayout.Toggle("敌人使用", card.enemyUse);
            if (card.playerUse != playerUse)
            {
                card.enemyUse = false;
                playerUse = card.playerUse;
            }
            else if (card.enemyUse != enemyUse)
            {
                card.playerUse = false;
                enemyUse = card.enemyUse;
            }
        }

        EditorGUILayout.Space(2);
        b0 = EditorGUILayout.Foldout(b0, "卡牌信息");
        if (b0)
        {
            card.id = EditorGUILayout.IntField("卡牌ID", card.id);
            card.name = EditorGUILayout.TextField("卡牌名称", card.name);
            card.rarity = (int)EditorGUILayout.Slider("卡牌稀有度", card.rarity, 1, 5);
            card.description = EditorGUILayout.DelayedTextField("卡牌描述", card.description, GUILayout.Height(100));
            card.backGroundStory = EditorGUILayout.DelayedTextField("卡牌背景故事", card.backGroundStory, GUILayout.Height(100));
        }
        EditorGUILayout.Space(2);

        b1 = EditorGUILayout.Foldout(b1, "作用对象");
        if (b1)
        {
            card.effortOnPlayer = EditorGUILayout.Toggle("作用于玩家", card.effortOnPlayer);
            card.effortOnEnmey = EditorGUILayout.Toggle("作用于敌人", card.effortOnEnmey);
            card.effortOnPlayerAndEnemy = EditorGUILayout.Toggle("作用于玩家与敌人", card.effortOnPlayerAndEnemy);
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
            if (card.effortOnPlayerAndEnemy)
            {
                card.effortOnPlayer = false;
                card.effortOnEnmey = false;
            }
            if (card.effortOnEnmey || card.effortOnPlayerAndEnemy)
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
        if (card.playerUse)
        {
            EditorGUILayout.Space(2);

            b2 = EditorGUILayout.Foldout(b2, "作用方法");
            if (b2)
            {
                card.click = EditorGUILayout.Toggle("点击", card.click);
                if (card.click)
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
        }
        EditorGUILayout.Space(2);

        b3 = EditorGUILayout.Foldout(b3, "作用效果");
        if (b3)
        {
            if (!card.effortOnPlayerAndEnemy)
            {

                serializedObject.Update();
                if (card.playerUse)
                {
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
                }
                EditorGUILayout.PropertyField(buff, new GUIContent("需要添加的buff"), true);
                //for (int i = 0; i < buff.arraySize; i++)
                //{
                //    SerializedProperty element=buff.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField (element,new GUIContent("Element"+i));
                //}
                EditorGUILayout.PropertyField(buffName, new GUIContent("需要添加的buff的名字"), true);
                //for (int i = 0; i < buffName.arraySize; i++)
                //{
                //    SerializedProperty element = buffName.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField(element, new GUIContent("Element" + i));
                //}
                if (buff != null && buffName != null)
                {
                    EditorGUILayout.HelpBox("注意:\"需要添加的buff\"与\"需要添加的buff的名字\"而这只需要填一个就行，如果这两上面个都填的是同一个buff那么将会对对象施加2次该buff", MessageType.Warning);
                }
            }
            else
            {
                serializedObject.Update();
                if (card.playerUse)
                {
                    EditorGUILayout.LabelField("敌人在卡牌被拖拽时所发生的事件");
                    EditorGUILayout.PropertyField(event11);
                    EditorGUILayout.LabelField("敌人在鼠标在卡牌上所发生的事件");
                    EditorGUILayout.PropertyField(event12);
                    EditorGUILayout.LabelField("敌人在鼠标离开卡牌所发生的事件");
                    EditorGUILayout.PropertyField(event13);
                    EditorGUILayout.LabelField("敌人在卡牌发生作用时所发生的事件");
                    EditorGUILayout.PropertyField(event14);
                    EditorGUILayout.LabelField("敌人在选择角色作为卡牌作用目标时所发生的事件");
                    EditorGUILayout.PropertyField(event15);
                }
                EditorGUILayout.PropertyField(buff2, new GUIContent("敌人需要添加的buff"), true);
                //for (int i = 0; i < buff.arraySize; i++)
                //{
                //    SerializedProperty element=buff.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField (element,new GUIContent("Element"+i));
                //}
                EditorGUILayout.PropertyField(buffName2, new GUIContent("敌人需要添加的buff的名字"), true);
                //for (int i = 0; i < buffName.arraySize; i++)
                //{
                //    SerializedProperty element = buffName.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField(element, new GUIContent("Element" + i));
                //}
                if (buff != null && buffName != null)
                {
                    EditorGUILayout.HelpBox("注意:\"需要添加的buff\"与\"需要添加的buff的名字\"而这只需要填一个就行，如果这两上面个都填的是同一个buff那么将会对对象施加2次该buff", MessageType.Warning);
                }
                if (card.playerUse)
                {
                    EditorGUILayout.LabelField("玩家在卡牌被拖拽时所发生的事件");
                    EditorGUILayout.PropertyField(event6);
                    EditorGUILayout.LabelField("玩家在鼠标在卡牌上所发生的事件");
                    EditorGUILayout.PropertyField(event7);
                    EditorGUILayout.LabelField("玩家在鼠标离开卡牌所发生的事件");
                    EditorGUILayout.PropertyField(event8);
                    EditorGUILayout.LabelField("玩家在卡牌发生作用时所发生的事件");
                    EditorGUILayout.PropertyField(event9);
                    EditorGUILayout.LabelField("玩家在选择角色作为卡牌作用目标时所发生的事件");
                    EditorGUILayout.PropertyField(event10);
                }
                EditorGUILayout.PropertyField(buff1, new GUIContent("玩家需要添加的buff"), true);
                //for (int i = 0; i < buff.arraySize; i++)
                //{
                //    SerializedProperty element=buff.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField (element,new GUIContent("Element"+i));
                //}
                EditorGUILayout.PropertyField(buffName1, new GUIContent("玩家需要添加的buff的名字"), true);
                //for (int i = 0; i < buffName.arraySize; i++)
                //{
                //    SerializedProperty element = buffName.GetArrayElementAtIndex(i);
                //    EditorGUILayout.PropertyField(element, new GUIContent("Element" + i));
                //}
                if (buff != null && buffName != null)
                {
                    EditorGUILayout.HelpBox("注意:\"需要添加的buff\"与\"需要添加的buff的名字\"而这只需要填一个就行，如果这两上面个都填的是同一个buff那么将会对对象施加2次该buff", MessageType.Warning);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(card);
    }
}
