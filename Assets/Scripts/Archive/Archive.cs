using System;
using UnityEngine;

[System.Serializable]
public class Archive
{
    public int index;
    public int level;
    public int currentExp;
    public int maxExp;
    public int feather;
    public int dream;
    public int tenacity;
    public int strength;
    public ItemInfos equipedFeather;
    public ItemInfos equipedBrokenFeather;
    public ItemInfos items;
}

/// <summary>
/// 物品信息
/// </summary>
[Serializable]
public struct ItemInfo
{
    public string itemName;
    public string information;

    public ItemType type;

    public bool isDreamizable;
    public int dreamizeCost;

    public string imageName;

    public int position;
}

[Serializable]
public struct ItemInfos
{
    public ItemInfo[] items;
}