using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// 数据配置基类
public abstract class ConfigBase
{
    public int id;
    public string name;
}

// 角色配置
[Serializable]
public class CharacterConfig : ConfigBase
{
    public int initialHP;
    public int initialAttack;
    public int initialDefense;
    public int initialSpeed;
    public string prefabPath;      // 角色模型路径
    public string iconPath;        // 角色图标路径
    public List<int> skillIds;     // 技能ID列表
    public GrowthConfig growth;    // 成长属性
    public int quality;            // 角色品质
    public CharacterType type;     // 角色类型
}

// 角色成长配置
[Serializable]
public class GrowthConfig
{
    public float hpGrowth;         // 生命值成长
    public float attackGrowth;     // 攻击力成长
    public float defenseGrowth;    // 防御力成长
    public float speedGrowth;      // 速度成长
}

// 技能效果
[Serializable]
public class SkillEffect
{
    public EffectType type;        // 效果类型
    public float value;            // 效果数值
    public int duration;           // 持续回合
    public TargetType target;      // 目标类型
}

// 抽卡配置
[Serializable]
public class GachaConfig : ConfigBase
{
    public int type;               // 消耗货币种类
    public int cost;               // 消耗货币数量
    public List<GachaRate> rates;  // 抽卡概率配置
}

// 抽卡概率配置
[Serializable]
public class GachaRate
{
    public int quality;            // 品质
    public float rate;             // 概率
    public List<int> characterIds; // 可能出现的角色ID列表
}

// 玩家数据
[Serializable]
public class PlayerData
{
    public int playerId;
    public string playerName;
    public int level;
    public int exp;
    public int coin;              // 游戏币
    public int diamond;           // 钻石
    public List<CharacterData> characters; // 拥有的角色
    public List<int> battleTeam;  // 战斗队伍
}

// 角色数据
[Serializable]
public class CharacterData
{
    public int configId;          // 对应配置ID
    public int level;             // 等级
    public int exp;               // 经验值
    public List<int> equipment;   // 装备列表
    public List<int> skills;      // 技能列表
}

// 战斗数据
[Serializable]
public class BattleData
{
    public int battleId;
    public BattleType battleType;
    public List<int> playerTeam;
    public List<int> enemyTeam;
    public Dictionary<int, BattleUnitData> unitDataDict;
}

// 战斗单位数据
[Serializable]
public class BattleUnitData
{
    public int configId;
    public int currentHp;
    public int currentMp;
    public List<BuffData> buffs;
    public Dictionary<int, int> skillCDs;
}

// Buff数据
[Serializable]
public class BuffData
{
    public int buffId;
    public int duration;
    public float value;
}

// 枚举定义
public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

public enum CharacterType
{
    Tank,
    Warrior,
    Mage,
    Support,
    Archer
}

public enum EffectType
{
    Damage,
    Heal,
    Buff,
    Debuff,
    Control
}

public enum TargetType
{
    Self,
    SingleEnemy,
    AllEnemy,
    SingleAlly,
    AllAlly
}

public enum BattleType
{
    PVE,
    PVP,
    BOSS
}

// 配置管理器
public class ConfigManager : Singleton<ConfigManager>
{
    private Dictionary<Type, Dictionary<int, ConfigBase>> configDict = new Dictionary<Type, Dictionary<int, ConfigBase>>();
    private Dictionary<Type, Dictionary<int, object>> configs = new Dictionary<Type, Dictionary<int, object>>();

    public void Initialize()
    {
        LoadAllConfigs();
    }

    private void LoadAllConfigs()
    {
        LoadConfig<CharacterConfig>("Characters");
        LoadConfig<GachaConfig>("Gacha");
    }

    private void LoadConfig<T>(string fileName) where T : ConfigBase
    {
        // 从本地或服务器加载配置
        // 可以支持热更新配置
    }

    public T GetConfig<T>(int id) where T : ConfigBase
    {
        if (configDict.TryGetValue(typeof(T), out var dict))
        {
            if (dict.TryGetValue(id, out var config))
            {
                return config as T;
            }
        }
        return null;
    }


    public TConfig[] GetAllConfigs<TConfig>() where TConfig : class
    {
        if (configs.TryGetValue(typeof(TConfig), out var configDict))
        {
            return configDict.Values.Cast<TConfig>().ToArray();
        }
        return Array.Empty<TConfig>();
    }
}