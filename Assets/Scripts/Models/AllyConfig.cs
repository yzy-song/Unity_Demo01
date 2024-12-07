using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyInfo
{
    public int id;          // 角色id
    public string type;          // 角色类型
    public int level;            // 等级
    public Vector2 position;     // 初始位置
    public List<string> skills;  // 技能列表
}

[System.Serializable]
public class AllyData
{
    public List<AllyInfo> allies; // 友方角色列表
}

public class AllyConfig : MonoBehaviour
{
    public AllyData allyData;

    public void LoadAllyConfig()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("AlliedUnits");
        allyData = JsonUtility.FromJson<AllyData>(jsonFile.text);
    }

    public List<AllyInfo> GetAllies()
    {
        return allyData.allies;
    }
}
