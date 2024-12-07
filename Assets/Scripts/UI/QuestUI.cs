// 文件路径：Scripts/UI/QuestUI.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questItemPrefab; // QuestItem 预制体
    [SerializeField] private ScrollRect questScrollView;        // 任务列表的 Scroll View
    [SerializeField] private int poolSize = 20;                 // 对象池大小
    private ObjectPool<GameObject> questItemPool;               // 对象池
    private void Start()
    {
        // 初始化对象池
        questItemPool = new ObjectPool<GameObject>(
            () => Instantiate(questItemPrefab),
            poolSize
        );

        UpdateQuestList();

    }

    public void UpdateQuestList()
    {
        // 清空任务列表内容
        foreach (Transform child in questScrollView.content)
        {
            questItemPool.Release(child.gameObject);
        }

        // 获取所有任务
        var quests = QuestManager.Instance.GetAllQuests();

        // 动态生成任务项
        foreach (var quest in quests)
        {
            var questItem = questItemPool.Get();
            questItem.transform.SetParent(questScrollView.content, false);
            questItem.SetActive(true);

            // 设置任务数据
            var questItemUI = questItem.GetComponent<QuestItem>();
            questItemUI.Initialize(quest);
        }
    }

    public void OnClose()
    {
        UIManager.Instance.HidePanel("QuestPanel");
    }


}
