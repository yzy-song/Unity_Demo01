using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();

    protected override void Awake()
    {
        Debug.Log("QuestManager Awake");
        base.Awake();
        LoadQuests();
    }

    // 加载任务数据
    public void LoadQuests()
    {
        TextAsset questJson = Resources.Load<TextAsset>("Quests");
        if (questJson == null)
        {
            Debug.LogError("Quests.json 文件未找到");
            return;
        }

        QuestList questConfig = JsonUtility.FromJson<QuestList>(questJson.text);
        foreach (Quest quest in questConfig.quests)
        {
            questDictionary[quest.questID] = quest;
            Debug.Log($"任务加载: ID: {quest.questID}, Title: {quest.title}");
        }

        Debug.Log("任务数据加载完成");
    }

    // 获取任务
    public Quest GetQuestById(string questID)
    {
        if (questDictionary.TryGetValue(questID, out var quest))
        {
            return quest;
        }

        Debug.LogError($"未找到任务 ID: {questID}");
        return null;
    }

    // 更新任务进度
    public void UpdateQuestProgress(string questID, int progress)
    {
        if (questDictionary.TryGetValue(questID, out var quest))
        {
            quest.currentProgress += progress;
            if (quest.currentProgress >= quest.targetAmount)
            {
                quest.isCompleted = true;
                Debug.Log($"任务完成: {quest.title}");
                RewardPlayer(quest);
            }
        }
        else
        {
            Debug.LogError($"无法更新任务进度，未找到任务 ID: {questID}");
        }
    }

    // 奖励玩家
    private void RewardPlayer(Quest quest)
    {
        Debug.Log($"玩家获得奖励: Gold {quest.rewards.Gold}, Exp {quest.rewards.Exp}");
        // PlayerManager.Instance.AddGold(quest.Rewards.Gold);
        // PlayerManager.Instance.AddExp(quest.Rewards.Exp);

        foreach (var itemId in quest.rewards.Items)
        {
            InventoryManager.Instance.AddItem(itemId, 1);
            Debug.Log($"获得物品: ID: {itemId}");
        }
    }

    // 获取所有任务
    public List<Quest> GetAllQuests()
    {
        return new List<Quest>(questDictionary.Values);
    }
}
