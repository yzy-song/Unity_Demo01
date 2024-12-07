using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public string allyType;
    public float maxHealth;
    public float currentHealth;
    public string characterName;
    public List<Skill> skills = new List<Skill>();
    public HealthBar healthBar;       // 血条

    private static SpineCharacterDatabase characters;
    public void Initialize(int characterId)
    {
        // 初始化角色
        TextAsset characterJson = Resources.Load<TextAsset>("SpineCharacters");
        characters = JsonUtility.FromJson<SpineCharacterDatabase>(characterJson.text);

        SpineCharacter character = characters.characters.Find(c => c.id == characterId);
        characterName = character.name;
        allyType = character.type;
        // maxHealth = 100;
        // currentHealth = maxHealth;
        healthBar.Initialize(maxHealth);
        foreach (var skillId in character.skills)
        {
            Skill skill = SkillManager.Instance.GetSkillById(skillId);

            if (skill != null)
            {
                skills.Add(skill);
            }
        }

        Debug.Log($"{characterName} 技能加载完成");
    }

    public void UseSkill(Skill selectedSkill, MonsterController targetEnemy, System.Action onComplete)
    {
        // 播放技能动画和音效
        Debug.Log($"{allyType} 使用技能对 {targetEnemy.name}");
        // 示例：技能动画模拟
        StartCoroutine(SimulateSkillAnimation(selectedSkill, targetEnemy, onComplete));
    }

    private System.Collections.IEnumerator SimulateSkillAnimation(Skill selectedSkill, MonsterController targetEnemy, System.Action onComplete)
    {
        // 等待1秒模拟技能动画播放
        yield return new WaitForSeconds(1f);

        // 使用技能伤害值
        targetEnemy.TakeDamage(selectedSkill.damage);
        Debug.Log($"{targetEnemy.name} 受到 {selectedSkill.damage} 点伤害");

        // 调用完成回调
        onComplete?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // 更新血条
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }

        Debug.Log($"{allyType} 受到 {damage} 点伤害, 当前血量: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{allyType} 已死亡");
        // 执行死亡逻辑，例如移除角色或显示死亡动画
        Destroy(gameObject);
    }
}
