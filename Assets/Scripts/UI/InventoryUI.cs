using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ToggleGroup tabGroup;              // 页签 Toggle Group
    [SerializeField] private ScrollRect[] scrollViews;          // 每个页签对应的 Scroll View
    [SerializeField] private GameObject itemPrefab;             // 物品格子预制体
    [SerializeField] private int poolSize = 50;                 // 对象池大小

    [SerializeField] private Button btnClose;

    private ObjectPool<GameObject> itemPool;
    private int currentTabIndex = 0;                            // 当前页签索引

    void Start()
    {
        // 初始化对象池
        itemPool = new ObjectPool<GameObject>(
            () => Instantiate(itemPrefab),
            poolSize
        );

        // 注册背包界面到 UIManager
        UIManager.Instance.RegisterPanel("InventoryPanel", gameObject);

        // 配置物品 ID 范围，根据配置的物品数量调整
        int[] itemIds = { 1, 2, 3, 4, 5 };

        // 随机添加 100 个物品
        for (int i = 0; i < 100; i++)
        {
            // 随机选择一个物品 ID
            int randomItemId = itemIds[Random.Range(0, itemIds.Length)];

            // 随机选择一个页签（假设页签对应 rarity）
            int randomRarity = Random.Range(0, 3);

            // 设置物品的 rarity
            var item = InventoryManager.Instance.GetItemById(randomItemId);
            if (item != null)
            {
                item.rarity = randomRarity; // 修改稀有度
                InventoryManager.Instance.AddItem(randomItemId, Random.Range(1, 10)); // 随机数量
            }
        }

        Debug.Log("随机物品添加完成");
        // 初始化页签事件
        InitializeTabEvents();

        // 默认显示第一个页签内容
        UpdateInventoryUI(currentTabIndex);
    }

    private void InitializeTabEvents()
    {
        var toggles = tabGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            int tabIndex = i; // 防止闭包问题
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    currentTabIndex = tabIndex;
                    UpdateInventoryUI(currentTabIndex);
                }
            });
        }
    }

    private void UpdateInventoryUI(int tabIndex)
    {
        // 清空其他 Scroll View 的内容
        for (int i = 0; i < scrollViews.Length; i++)
        {
            foreach (Transform child in scrollViews[i].content)
            {
                itemPool.Release(child.gameObject);
            }
            scrollViews[i].gameObject.SetActive(i == tabIndex);
        }

        // 获取当前页签的物品
        var items = InventoryManager.Instance.GetItemsByCategory(tabIndex);

        // 动态生成物品格子
        foreach (var inventoryItem in items)
        {
            var itemUI = itemPool.Get();
            itemUI.transform.SetParent(scrollViews[tabIndex].content, false);
            itemUI.SetActive(true);

            var inventoryItemUI = itemUI.GetComponent<InventoryItemUI>();
            inventoryItemUI.SetData(inventoryItem);
        }
    }

    public void OnClose()
    {
        UIManager.Instance.HidePanel("InventoryPanel");
    }
}
