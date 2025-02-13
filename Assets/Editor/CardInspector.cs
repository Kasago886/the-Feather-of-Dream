using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
[CustomEditor(typeof(Card), true)]
public class CardInspector : Editor
{

    private Card card;
    private SerializedProperty event1, event2, event3, event4, event5, buff, buffName, event6, event7, event8, event9, event10, buff1, buffName1, event11, event12, event13, event14, event15, buff2, buffName2,position,materia;
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
        position= serializedObject.FindProperty("targetTransform");
        materia= serializedObject.FindProperty("speacialMaterial");
    }
    public override void OnInspectorGUI()
    {
        Undo.RecordObject(card, "Change Card");
        serializedObject.Update();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(2);
        card.b4 = EditorGUILayout.Foldout(card.b4, "使用者");
        if (card.b4)
        {
            card.playerUse = EditorGUILayout.Toggle("玩家使用", card.playerUse);
            if (card.playerUse )
            {
                EditorGUILayout.LabelField("请放入Custom_UI_SpriteGlowBorder");
                EditorGUILayout.PropertyField(materia);
            }
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
        card.b0 = EditorGUILayout.Foldout(card.b0, "卡牌信息");
        if (card.b0)
        {
            card.id = EditorGUILayout.IntField("卡牌ID", card.id);
            card.name = EditorGUILayout.TextField("卡牌名称", card.name);
            card.rarity = (int)EditorGUILayout.Slider("卡牌稀有度", card.rarity, 1, 5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("卡牌描述", GUILayout.Width(50));
            card.description = EditorGUILayout.TextArea(card.description, GUILayout.Height(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("卡牌背景\n故事", GUILayout.Width(50), GUILayout.Height(50));
            card.backGroundStory = EditorGUILayout.TextArea(card.backGroundStory, GUILayout.Height(100));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space(2);

        card.b1 = EditorGUILayout.Foldout(card.b1, "作用对象");
        if (card.b1)
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

            card.b2 = EditorGUILayout.Foldout(card.b2, "作用方法");
            if (card.b2)
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

        card.b3 = EditorGUILayout.Foldout(card.b3, "作用效果");
        if (card.b3)
        {
            if (!card.effortOnPlayerAndEnemy)
            {
                if (card.playerUse)
                {
                    EditorGUILayout.LabelField("在卡牌被拖拽时所发生的事件");
                    EditorGUILayout.PropertyField(event1);
                    EditorGUILayout.LabelField("在鼠标在卡牌上所发生的事件");
                    EditorGUILayout.PropertyField(event2);
                    EditorGUILayout.LabelField("在鼠标离开卡牌所发生的事件");
                    EditorGUILayout.PropertyField(event3);
                    EditorGUILayout.LabelField("在选择角色作为卡牌作用目标时所发生的事件");
                    EditorGUILayout.PropertyField(event5);
                }
                EditorGUILayout.LabelField("在卡牌发生作用时所发生的事件");
                EditorGUILayout.PropertyField(event4);
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
                if (card.playerUse)
                {
                    EditorGUILayout.LabelField("敌人在卡牌被拖拽时所发生的事件");
                    EditorGUILayout.PropertyField(event11);
                    EditorGUILayout.LabelField("敌人在鼠标在卡牌上所发生的事件");
                    EditorGUILayout.PropertyField(event12);
                    EditorGUILayout.LabelField("敌人在鼠标离开卡牌所发生的事件");
                    EditorGUILayout.PropertyField(event13);
                    EditorGUILayout.LabelField("敌人在选择角色作为卡牌作用目标时所发生的事件");
                    EditorGUILayout.PropertyField(event15);
                }
                EditorGUILayout.LabelField("敌人在卡牌发生作用时所发生的事件");
                EditorGUILayout.PropertyField(event14);
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
                    EditorGUILayout.LabelField("玩家在选择角色作为卡牌作用目标时所发生的事件");
                    EditorGUILayout.PropertyField(event10);
                }
                EditorGUILayout.LabelField("玩家在卡牌发生作用时所发生的事件");
                EditorGUILayout.PropertyField(event9);
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
        }
        card.b5 = EditorGUILayout.Foldout(card.b5, "自定义判定范围");
        if (card.b5)
        {
            EditorGUILayout.HelpBox("注意:此处修改的为敌人的判定范围，默认判定范围为屏幕内，如需修改请勾选下方自定义判定范围", MessageType.Warning);
            card.customMode = EditorGUILayout.Toggle("自定义判定范围", card.customMode);
            card.pleaseChooseOneMethod=(targetMethod)EditorGUILayout.EnumPopup("中心位置", card.pleaseChooseOneMethod);
            if(card.pleaseChooseOneMethod==0)
            {
                EditorGUILayout.LabelField("中心物体");
                EditorGUILayout.PropertyField(position);
            }
            else
            {
                card.theNumberOfTargetPosition=EditorGUILayout.Vector3Field("中心坐标", card.theNumberOfTargetPosition);
            }
            card.getObjectDistanceInX=EditorGUILayout.FloatField("X方向上的范围", card.getObjectDistanceInX);
            card.getObjectDistanceInY = EditorGUILayout.FloatField("Y方向上的范围", card.getObjectDistanceInY);
        }
        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(card);
        }
    }
}
