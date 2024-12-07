using System;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemId;
    public string itemName;
    public string iconPath; // 图标路径
    public string description;
    public int rarity;
    public int quantity; // 数量

    // 动态加载的资源
    [NonSerialized] public Sprite icon;
}
[System.Serializable]
public class ItemList
{
    public Item[] items;
}

