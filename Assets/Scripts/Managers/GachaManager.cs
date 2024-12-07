using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 抽卡管理器
public class GachaManager : Singleton<GachaManager>
{
    private Dictionary<int, GachaConfig> gachaConfigs;
    private System.Random random;
    
    public void Initialize()
    {
        random = new System.Random();
        LoadGachaConfigs();
    }
    
    private void LoadGachaConfigs()
    {
        gachaConfigs = new Dictionary<int, GachaConfig>();
        var configs = ConfigManager.Instance.GetAllConfigs<GachaConfig>();
        foreach (var config in configs)
        {
            gachaConfigs[config.id] = config;
        }
    }
    
    public GachaResult DrawOnce(int gachaId)
    {
        if (!gachaConfigs.TryGetValue(gachaId, out var config))
            throw new ArgumentException("Invalid gacha id");
            
        // 检查货币是否足够
        if (!CheckCurrency(config.cost))
            throw new InvalidOperationException("Insufficient currency");
            
        // 扣除货币
        ConsumeCurrency(config.cost);
        
        // 抽卡
        var result = DrawCharacter(config);
        
        // 发放奖励
        GrantReward(result);
        
        return result;
    }
    
    public List<GachaResult> DrawMulti(int gachaId, int times)
    {
        if (!gachaConfigs.TryGetValue(gachaId, out var config))
            throw new ArgumentException("Invalid gacha id");
            
        // 检查货币是否足够
        if (!CheckCurrency(config.cost * times))
            throw new InvalidOperationException("Insufficient currency");
            
        // 扣除货币
        ConsumeCurrency(config.cost * times);
        
        var results = new List<GachaResult>();
        for (int i = 0; i < times; i++)
        {
            results.Add(DrawCharacter(config));
        }
        
        // 发放奖励
        foreach (var result in results)
        {
            GrantReward(result);
        }
        
        return results;
    }
    
    private GachaResult DrawCharacter(GachaConfig config)
    {
        // 计算权重总和
        float totalWeight = config.rates.Sum(r => r.rate);
        
        // 随机值
        float randomValue = (float)(random.NextDouble() * totalWeight);
        
        // 根据权重选择品质
        float currentWeight = 0;
        GachaRate selectedRate = null;
        foreach (var rate in config.rates)
        {
            currentWeight += rate.rate;
            if (randomValue <= currentWeight)
            {
                selectedRate = rate;
                break;
            }
        }
        
        // 从选中品质中随机选择角色
        int characterId = selectedRate.characterIds[random.Next(selectedRate.characterIds.Count)];
        
        return new GachaResult
        {
            characterId = characterId,
            quality = selectedRate.quality,
            isNew = !IsCharacterOwned(characterId)
        };
    }
    
    private bool CheckCurrency(int cost)
    {
        var playerData = DataManager.Instance.GetPlayerData();
        return playerData.diamond >= cost;
    }
    
    private void ConsumeCurrency(int cost)
    {
        DataManager.Instance.UpdatePlayerData(data =>
        {
            data.diamond -= cost;
        });
    }
    
    private bool IsCharacterOwned(int characterId)
    {
        var playerData = DataManager.Instance.GetPlayerData();
        return playerData.characters.Any(c => c.configId == characterId);
    }
    
    private void GrantReward(GachaResult result)
    {
        DataManager.Instance.UpdatePlayerData(data =>
        {
            if (result.isNew)
            {
                // 添加新角色
                data.characters.Add(new CharacterData
                {
                    configId = result.characterId,
                    level = 1,
                    exp = 0,
                    equipment = new List<int>(),
                    skills = ConfigManager.Instance.GetConfig<CharacterConfig>(result.characterId).skillIds
                });
            }
            else
            {
                // 转化为其他奖励（如碎片）
                // TODO: 实现重复角色的处理逻辑
            }
        });
    }
}

// 抽卡结果
public class GachaResult
{
    public int characterId;
    public int quality;
    public bool isNew;
}