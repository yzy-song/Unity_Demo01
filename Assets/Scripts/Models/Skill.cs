using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public int skillId;                  // 技能唯一标识
    public string skillName;             // 技能名称
    public float damage;                 // 技能伤害值
    public SkillTargetType targetType;   // 目标类型（单体/群体）
    public string icon;             // 技能图标名称
    public string effectPrefabPath; // 特效预制体路径
    public GameObject effectPrefab; // 特效预制体对象
    public string soundPath; // 音效路径
    public AudioClip sound; // 音效对象

    public void Execute(Transform caster, Transform target, System.Action onComplete)
    {
        Debug.Log($"技能 {skillName} 正在释放");

        // 播放特效
        if (effectPrefab != null)
        {
            GameObject effect = GameObject.Instantiate(effectPrefab, target.position, Quaternion.identity);
            GameObject.Destroy(effect, 2f); // 销毁特效
        }

        // 播放音效
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, caster.position);
        }

        // 对目标造成伤害
        if (target.TryGetComponent<MonsterController>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }

        // 技能完成回调
        onComplete?.Invoke();
    }
}

[System.Serializable]
public class SkillList
{
    public List<Skill> skills;
}
public enum SkillTargetType
{
    SingleTarget,
    AllEnemies
}
