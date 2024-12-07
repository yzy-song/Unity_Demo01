using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private AllyConfig allyConfig;     // 友方角色配置
    public List<AllyController> Allies { get; private set; } = new List<AllyController>();
    public List<MonsterController> Enemies { get; private set; } = new List<MonsterController>();

    public void RegisterAlly(AllyController ally)
    {
        Allies.Add(ally);
    }

    public void RegisterEnemy(MonsterController enemy)
    {
        Enemies.Add(enemy);
    }
    public BattleStateMachine StateMachine { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        // 初始化状态机
        StateMachine = new BattleStateMachine();

        BattleManager battleManager = BattleManager.Instance;

        // 实例化状态
        var prepareState = new BattleStatePrepare();
        var playerTurnState = new BattleStatePlayerTurn();
        var enemyTurnState = new BattleStateEnemyTurn();

        // 初始化状态
        prepareState.Initialize(battleManager);
        playerTurnState.Initialize(battleManager);
        enemyTurnState.Initialize(battleManager);

        // 注册状态
        StateMachine.AddState(prepareState);
        StateMachine.AddState(playerTurnState);
        StateMachine.AddState(enemyTurnState);

    }

    private void Start()
    {
        // 初始化为准备阶段
        StateMachine.Initialize<BattleStatePrepare>();
    }

    private void Update()
    {
        // 更新当前状态
        StateMachine.CurrentState?.UpdateState();
    }

    public void ChangeState(IBattleState newState)
    {
        StateMachine.ChangeState(newState);
    }

    // public List<GameObject> spawnedAllies { get; private set; } = new List<GameObject>();
    public void SpawnAllies()
    {
        allyConfig.LoadAllyConfig(); // 加载友方角色配置

        foreach (var allyInfo in allyConfig.GetAllies())
        {
            // 实例化友方角色
            SpineCharacter character = SpineCharacterDatabase.Instance.GetCharacterById(allyInfo.id);
            if (character == null)
            {
                Debug.LogError($"Character ID {allyInfo.id} not found!");
                continue;
            }
            // 加载预制体
            GameObject prefab = Resources.Load<GameObject>(character.prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {character.prefabPath}");
                continue;
            }
            // 实例化怪物
            GameObject ally = Instantiate(prefab);

            // 初始化角色属性
            AllyController allyController = ally.GetComponent<AllyController>();
            allyController.Initialize(allyInfo.id);

            ally.transform.SetParent(GameObject.Find(character.type).transform, false);

            // 添加到友方角色列表
            RegisterAlly(allyController);
        }

        HorizontalLayout2D layout = allyConfig.GetComponent<HorizontalLayout2D>();
        if (layout != null)
        {
            layout.UpdateLayout();
        }

        Debug.Log($"Spawned {Allies.Count} allies.");

        // 初始化第一个角色的技能
        if (Allies.Count > 0)
        {
            SetActiveAlly(Allies[0]);
        }
    }
    public int currentAllyIndex = 0; // 当前行动角色索引

    public void SetActiveAlly(AllyController ally)
    {
        // 更新 UI 显示当前角色技能
        UIManager.Instance.ShowSkillButtons(ally);
        Debug.Log($"Current active ally: {ally.name}");
    }

    public void NextAlly()
    {
        if (Allies.Count == 0) return;

        // 切换到下一个角色
        currentAllyIndex = (currentAllyIndex + 1) % Allies.Count;
        SetActiveAlly(Allies[currentAllyIndex]);
    }


    public List<AllyController> GetSpawnedAllies()
    {
        return Allies; // 返回所有生成的友方角色
    }

    // public List<GameObject> spawnedMonsters { get; private set; } = new List<GameObject>();

    public void SpawnMonsters(int waveIndex)
    {
        WaveInfo waveInfo = MonsterWaveConfig.Instance.GetWaveInfo(waveIndex);
        if (waveInfo == null)
        {
            Debug.LogError($"Wave {waveIndex} not found!");
            return;
        }

        foreach (var monsterInfo in waveInfo.monsters)
        {
            for (int i = 0; i < monsterInfo.count; i++)
            {
                // 获取怪物角色信息
                SpineCharacter character = SpineCharacterDatabase.Instance.GetCharacterById(monsterInfo.id);
                if (character == null)
                {
                    Debug.LogError($"Character ID {monsterInfo.id} not found!");
                    continue;
                }

                // 加载预制体
                GameObject prefab = Resources.Load<GameObject>(character.prefabPath);
                if (prefab == null)
                {
                    Debug.LogError($"Prefab not found at path: {character.prefabPath}");
                    continue;
                }

                // 实例化怪物
                GameObject monster = Instantiate(prefab);

                // 设置位置
                if (monsterInfo.positions != null && monsterInfo.positions.Count > i)
                {
                    Vector2 pos = monsterInfo.positions[i];
                    monster.transform.position = new Vector3(pos.x, pos.y, 0);
                }
                else
                {
                    // 如果没有指定位置，随机生成
                    monster.transform.position = GetRandomPosition();
                }

                monster.transform.SetParent(GameObject.Find(character.type).transform, false);
                // 调整敌人 Z 坐标，确保在背景之上
                monster.transform.localPosition = new Vector3(monster.transform.position.x, monster.transform.position.y, -1);

                // 初始化怪物属性
                MonsterController controller = monster.GetComponent<MonsterController>();
                if (controller != null)
                {
                    controller.Initialize(monsterInfo.level);
                }
                Debug.Log($"生成敌人: {controller.name} 血量 {controller.maxHealth}");
                RegisterEnemy(controller);
            }
        }
    }

    public void ClearMonsters()
    {
        foreach (var monster in Enemies)
        {
            Destroy(monster.gameObject);
        }
        Enemies.Clear();
    }

    public void ClearAllies()
    {
        foreach (var ally in Allies)
        {
            Destroy(ally.gameObject);
        }
        Allies.Clear();
    }

    private Vector3 GetRandomPosition()
    {
        // 自定义随机位置生成逻辑
        return new Vector3(Random.Range(-3f, 3f), 0, 0);
    }

    public bool AreAllMonstersDefeated()
    {
        return Enemies.TrueForAll(monster => monster == null);
    }

    public bool AreAllAlliesDefeated()
    {
        return Allies.TrueForAll(ally => ally == null);
    }

    public void CheckBattleEnd()
    {
        // 检查是否所有敌方单位被消灭
        if (AreAllMonstersDefeated())
        {
            Debug.Log("所有敌方单位已消灭，玩家胜利！");
            // ShowResultPanel(true); // 玩家胜利
            return;
        }

        // 检查是否所有友方单位被消灭
        if (AreAllAlliesDefeated())
        {
            Debug.Log("所有友方单位已消灭，敌人胜利！");
            // ShowResultPanel(false); // 敌人胜利
            return;
        }
    }

}
