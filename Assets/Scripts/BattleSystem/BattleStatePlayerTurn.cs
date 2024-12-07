using UnityEngine;

public class BattleStatePlayerTurn : IBattleState
{
    private BattleManager battleManager;
    private Skill selectedSkill; // 玩家选择的技能

    public void Initialize(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    public void EnterState()
    {
        Debug.Log("玩家回合开始");
        if (battleManager.Allies.Count > 0)
        {
            // 设置当前角色
            var currentAlly = battleManager.Allies[0];
            battleManager.SetActiveAlly(currentAlly);

            // 显示技能按钮
            UIManager.Instance.ShowSkillButtons(currentAlly);

            // 默认选择第一个技能
            if (currentAlly.skills.Count > 0)
            {
                OnSkillSelected(currentAlly.skills[0]);
            }
        }
    }


    public void UpdateState() { }

    public void ExitState()
    {
        Debug.Log("玩家回合结束");
    }

    public void OnSkillSelected(Skill skill)
    {
        Debug.Log($"玩家选择了技能: {skill.skillName}" + "等待玩家选择目标敌人");
        selectedSkill = skill;

    }

    public void OnEnemyTargetSelected(MonsterController targetEnemy)
    {
        // 释放技能
        battleManager.Allies[battleManager.currentAllyIndex].UseSkill(selectedSkill, targetEnemy, OnSkillAnimationComplete);
    }

    private void OnSkillAnimationComplete()
    {
        Debug.Log("技能动画播放完成，切换到下一个角色");

        // 递增当前友方角色索引
        battleManager.currentAllyIndex++;

        // 检查是否还有未行动的友方角色
        if (battleManager.currentAllyIndex < battleManager.Allies.Count)
        {
            Debug.Log($"切换到下一个友方角色：{battleManager.Allies[battleManager.currentAllyIndex].name}");

            // 设置下一个角色
            var nextAlly = battleManager.Allies[battleManager.currentAllyIndex];
            battleManager.SetActiveAlly(nextAlly);

            // 显示技能按钮并默认选择第一个技能
            UIManager.Instance.ShowSkillButtons(nextAlly);
            if (nextAlly.skills.Count > 0)
            {
                OnSkillSelected(nextAlly.skills[0]);
            }
        }
        else
        {
            Debug.Log("所有友方角色行动完成，切换到敌人回合");

            // 重置友方角色索引
            battleManager.currentAllyIndex = 0;

            // 切换到敌人回合
            battleManager.StateMachine.ChangeState<BattleStateEnemyTurn>();
        }
    }

}
