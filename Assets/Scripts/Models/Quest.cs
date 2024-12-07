using System;
using System.Collections.Generic;

[Serializable]
public class Quest
{
    public string questID;          // 任务唯一标识
    public string title;            // 任务标题
    public string description;      // 任务描述
    public int targetAmount;        // 目标数量
    public int currentProgress;     // 当前进度
    public bool isCompleted;        // 是否完成
    public QuestRewards rewards;    // 奖励信息

    public Quest(string questId, string title, string description, int targetAmount, QuestRewards rewards)
    {
        this.questID = questId;
        this.title = title;
        this.description = description;
        this.targetAmount = targetAmount;
        this.rewards = rewards;
        this.currentProgress = 0;
        this.isCompleted = false;
    }
}

[Serializable]
public class QuestList
{
    public List<Quest> quests; // 任务列表
}
