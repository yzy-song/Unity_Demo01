using System;

[Serializable]
public class QuestRewards
{
    public int Gold;       // 金币奖励
    public int Exp;        // 经验奖励
    public int[] Items;    // 物品奖励的 ID

    public QuestRewards(int gold, int exp, int[] items)
    {
        Gold = gold;
        Exp = exp;
        Items = items;
    }
}
