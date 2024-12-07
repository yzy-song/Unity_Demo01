using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();
    private List<InventoryItem> inventoryItems;

    protected override void Awake()
    {
        Debug.Log("InventoryManager Awake");
        base.Awake();
        LoadItems();
    }

    // 加载物品数据
    public void LoadItems()
    {
        TextAsset itemJson = Resources.Load<TextAsset>("Items");
        if (itemJson == null)
        {
            Debug.LogError("Items.json 文件未找到");
            return;
        }

        ItemList itemConfig = JsonUtility.FromJson<ItemList>(itemJson.text);
        inventoryItems = new List<InventoryItem>();
        foreach (Item item in itemConfig.items)
        {
            itemDictionary[item.itemId] = item;

            // // 动态加载图标
            // if (!string.IsNullOrEmpty(item.iconPath))
            // {
            //     item.icon = Resources.Load<Sprite>(item.iconPath);
            //     if (item.icon == null)
            //     {
            //         Debug.LogWarning($"未找到物品图标: {item.iconPath}");
            //     }
            // }
            inventoryItems.Add(new InventoryItem(item, 1));
            Debug.Log($"Item ID: {item.itemId}, Name: {item.itemName}");
        }

        Debug.Log("物品数据加载完成");
    }

    // 根据 itemId 获取物品
    public Item GetItemById(int itemId)
    {
        if (itemDictionary.TryGetValue(itemId, out var item))
        {
            return item;
        }

        Debug.LogError($"未找到物品 ID: {itemId}");
        return null;
    }

    // 增加物品
    public void AddItem(int itemId, int quantity)
    {
        // 检查物品是否已存在
        var existingItem = inventoryItems.Find(i => i.item.itemId == itemId);
        if (existingItem != null)
        {
            // 增加数量
            existingItem.quantity += quantity;
        }
        else
        {
            // 如果不存在，则尝试从 itemDictionary 获取物品
            var newItem = new Item
            {
                itemId = itemId,
                itemName = $"Test Item {itemId}", // 假数据名称
                iconPath = "Icons/Default",      // 默认图标路径
                description = "A test item for debugging.",
                rarity = 1                       // 假设稀有度为 1
            };

            inventoryItems.Add(new InventoryItem(newItem, quantity));
        }

        Debug.Log($"物品 ID: {itemId}, 添加数量: {quantity}");
    }


    // 减少物品
    public void RemoveItem(int itemId, int quantity)
    {
        if (itemDictionary.TryGetValue(itemId, out var item))
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                item.quantity = 0;
                Debug.Log($"物品 {item.itemName} 已用完");
            }
            else
            {
                Debug.Log($"减少物品 {item.itemName}, 数量: {quantity}, 剩余数量: {item.quantity}");
            }
        }
        else
        {
            Debug.LogError($"未找到物品 ID: {itemId}");
        }
    }

    public List<InventoryItem> GetItemsByCategory(int category)
    {
        if (inventoryItems == null)
        {
            Debug.LogError("InventoryItems 列表未初始化");
            return new List<InventoryItem>();
        }
        List<InventoryItem> filteredItems = new List<InventoryItem>();
        foreach (var inventoryItem in inventoryItems) // inventoryItems 是 List<InventoryItem>
        {
            if (inventoryItem.item.rarity == category && inventoryItem.quantity > 0)
            {
                filteredItems.Add(inventoryItem);
            }
        }
        return filteredItems;
    }


}
