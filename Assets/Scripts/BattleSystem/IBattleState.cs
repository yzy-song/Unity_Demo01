public interface IBattleState
{
    void Initialize(BattleManager battleManager);
    void EnterState();
    void UpdateState();
    void ExitState();
    void OnEnemyTargetSelected(MonsterController targetEnemy); // 添加目标选择处理方法
    void OnSkillSelected(Skill skill); // 增加技能选择处理方法
}
