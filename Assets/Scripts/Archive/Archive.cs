using System;
using UnityEngine;

[System.Serializable]
public class Archive
{
    public int index;
    public LevelInfo levelInfo;
    public TimeInfo timeInfo;
    public PlayerInfo playerInfo;
    
    public ItemInfos equipedFeather;
    public ItemInfos equipedBrokenFeather;
    public ItemInfos items;
    public ItemInfos encyclopedia;
}

[Serializable]
public struct PlayerInfo
{
    public int level;
    public int currentExp;
    public int maxExp;
    public int feather;
    public int dream;
    public int tenacity;
    public int strength;
}
/// <summary>
/// 物品信息
/// </summary>
[Serializable]
public class ItemInfo
{
    public string itemName;
    public string information;

    public ItemType type;

    public bool isDreamizable;
    public int dreamizeCost;
    public ItemInfo dreamizedFeather;

    public string imageName;

    public string buffName;
    public float featherHealth;
    public float featherMaxHealth;

    public string dialogName;

    public int position;
}

[Serializable]
public struct ItemInfos
{
    public ItemInfo[] items;
}

[Serializable]
public struct TimeInfo
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
}

[Serializable]
public struct LevelInfo
{
    public int level;
    public string title;

    public int archivePoint;

    public bool tutorialDone;
    public bool littleRedRidingHood;
    public bool level11DreamBottleUsed;
    public bool level11DreamBottleUsed2;
    public bool weirdDwarf;
    public bool level21DreamBottleUsed;
    public bool level21StartDialogRead;
    public bool level31DreamBottleUsed;
    public bool whaleKilled;
    public bool tinWoodmanKilled;
    public bool fragPrinceKilled;
    public bool fragGuard1Killed;
    public bool fragGuard2Killed;
    public bool crazyHunterKilled;
    public bool burningDocuments1Killed;
    public bool burningDocuments2Killed;
    public bool wolfKilled;
    public bool finalBossKilled;
    public bool contaminateCloneKilled;
    public bool drNeprizKilled;
    public bool enslavedDwarfKilled;
    public bool dwarfKilled;

    public bool AssistantDestroyed;
}
