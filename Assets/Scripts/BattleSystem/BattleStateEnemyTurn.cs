using UnityEngine;

public class BattleStateEnemyTurn : IBattleState
{
    private BattleManager battleManager;
    private int currentEnemyIndex; // 当前行动的敌人索引

    // 初始化方法
    public void Initialize(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    // 进入状态
    public void EnterState()
    {
        Debug.Log("敌人回合开始");
        currentEnemyIndex = 0; // 从第一个敌人开始行动

        if (battleManager.Enemies.Count > 0)
        {
            StartEnemyAction();
        }
        else
        {
            EndEnemyTurn(); // 没有敌人时直接结束敌人回合
        }
    }

    // 更新状态（可选，不需要主动操作时可以留空）
    public void UpdateState() { }

    // 退出状态
    public void ExitState()
    {
        Debug.Log("敌人回合结束");
    }

    // 开始敌人行动
    private void StartEnemyAction()
    {
        if (currentEnemyIndex < battleManager.Enemies.Count)
        {
            var enemy = battleManager.Enemies[currentEnemyIndex];
            enemy.StartAction(OnEnemyActionComplete); // 当前敌人开始行动
        }
        else
        {
            EndEnemyTurn(); // 所有敌人行动完毕，结束敌人回合
        }
    }

    // 敌人行动完成后的回调
    private void OnEnemyActionComplete()
    {
        currentEnemyIndex++;
        StartEnemyAction(); // 开始下一个敌人的行动
    }

    // 结束敌人回合
    private void EndEnemyTurn()
    {
        battleManager.StateMachine.ChangeState<BattleStatePlayerTurn>(); // 切换到玩家回合
    }

    public void OnEnemyTargetSelected(MonsterController targetEnemy)
    {
        Debug.Log("敌人回合中无法选择目标");
        throw new System.NotImplementedException();
    }

    public void OnSkillSelected(Skill skill)
    {
        throw new System.NotImplementedException();
    }
}
