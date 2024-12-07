using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public int level = 1;
    public string enemyType;
    public float maxHealth;
    public float currentHealth;

    private void OnEnable()
    {
        ClickEvent.OnObjectClicked += HandleClick;
    }

    private void OnDisable()
    {
        ClickEvent.OnObjectClicked -= HandleClick;
    }
    private void HandleClick(GameObject clickedObject)
    {
        if (clickedObject == gameObject)
        {
            // 检查是否是玩家回合
            if (BattleManager.Instance.StateMachine.CurrentState is BattleStatePlayerTurn)
            {
                Debug.Log($"{gameObject.name} 被点击！");
                BattleManager.Instance.StateMachine.CurrentState.OnEnemyTargetSelected(this);
            }
            else
            {
                Debug.Log("当前不是玩家回合，禁止点击怪物");
            }
        }
    }
    public void Initialize(int monsterLevel)
    {
        level = monsterLevel;
        // 初始化其他属性，如血量、攻击力等
    }

    // 怪物行为逻辑
    void Update()
    {
        // 简单的待机或攻击行为
    }

    // 当怪物死亡时调用
    public void OnDefeated()
    {
        Destroy(gameObject);
    }

    public void StartAction(System.Action onComplete)
    {
        Debug.Log($"{enemyType} is taking action.");

        // 模拟简单攻击
        AttackRandomAlly();

        // 行动完成后回调
        onComplete?.Invoke();
    }

    private void AttackRandomAlly()
    {
        var allies = BattleManager.Instance.Allies;
        if (allies.Count > 0)
        {
            var target = allies[Random.Range(0, allies.Count)];
            target.TakeDamage(10); // 示例伤害
            Debug.Log($"{enemyType} attacked {target.name} for 10 damage.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log($"{enemyType} 剩余血量: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 已死亡！");
        PlayDissolveEffect(); // 播放溶解消散特效

        // 通知 BattleManager
        BattleManager.Instance.Enemies.Remove(this);
        BattleManager.Instance.CheckBattleEnd();

        Destroy(gameObject, 1f); // 延迟销毁以显示特效
    }
    private void PlayDissolveEffect()
    {
        // 示例：溶解特效
        var dissolveEffect = Resources.Load<GameObject>("Effects/DissolveEffect");
        if (dissolveEffect != null)
        {
            Instantiate(dissolveEffect, transform.position, Quaternion.identity);
        }
    }
}
