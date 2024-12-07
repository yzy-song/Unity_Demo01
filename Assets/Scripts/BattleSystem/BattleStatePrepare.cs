using UnityEngine;

public class BattleStatePrepare : IBattleState
{
    private BattleManager battleManager;
    public void Initialize(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
    public void EnterState()
    {
        Debug.Log("准备阶段，生成怪物");

        // 生成当前波次的怪物
        battleManager.SpawnMonsters(battleManager.StateMachine.currentWaveIndex);

        // 生成友方角色
        battleManager.SpawnAllies();

        // 切换到玩家回合
        battleManager.StateMachine.ChangeState<BattleStatePlayerTurn>();
    }

    public void UpdateState() { }

    public void ExitState() { }

    public void OnEnemyTargetSelected(MonsterController targetEnemy)
    {
        throw new System.NotImplementedException();
    }

    public void OnSkillSelected(Skill skill)
    {
        throw new System.NotImplementedException();
    }
}
