using UnityEngine;

public class BattleStateEnd : IBattleState
{
    private BattleManager battleManager;

    public void Initialize(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    public void EnterState()
    {
        Debug.Log("战斗结束！");
        // 显示胜利或失败
    }

    public void UpdateState()
    {
        // 无需更新
    }

    public void ExitState()
    {
        Debug.Log("退出战斗结束状态");
    }

    public void OnEnemyTargetSelected(MonsterController targetEnemy)
    {
        throw new System.NotImplementedException();
    }

    public void OnSkillSelected(Skill skill)
    {
        throw new System.NotImplementedException();
    }
}
