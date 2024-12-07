using System.Collections.Generic;

[System.Serializable]
public class SpineCharacter
{
    public int id;                // 唯一 ID
    public string name;           // 角色名称
    public string prefabPath;     // 预制体路径
    public string type;           // 类型：Enemy 或 Ally
    public List<int> skills; // 技能 ID 列表
}
