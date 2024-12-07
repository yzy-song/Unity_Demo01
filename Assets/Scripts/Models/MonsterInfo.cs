using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public int id;                      // 怪物ID
    public int level;                   // 怪物等级
    public int count;                   // 怪物数量
    public List<Vector2> positions;     // 怪物位置列表
}
