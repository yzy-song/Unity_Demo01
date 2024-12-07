using System;
using UnityEngine; 
using System.Collections.Generic;

// 数据管理器 - 负责游戏数据的存储和读取
public class DataManager : Singleton<DataManager>
{

    private PlayerData playerData;
    private BattleData currentBattle;

    private Dictionary<string, object> gameData = new Dictionary<string, object>();

    public void Initialize()
    {
        // 初始化数据存储系统
        LoadGameData();
    }

    private void LoadGameData()
    {
        // 加载本地存储的游戏数据
    }

    public void SaveGameData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        // 保存到本地或发送到服务器
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData);
        // 保存到本地或发送到服务器
    }

    public void LoadPlayerData()
    {
        // 从本地或服务器加载数据
    }

    public void UpdatePlayerData(Action<PlayerData> updateAction)
    {
        updateAction?.Invoke(playerData);
        SavePlayerData();
    }

    public CharacterData GetCharacterData(int configId)
    {
        return playerData.characters.Find(c => c.configId == configId);
    }

    public PlayerData GetPlayerData()
    {
        // 获取当前玩家的数据
        return playerData;
    }
}