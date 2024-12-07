using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text playerHealthText;
    public Text enemyHealthText;
    public Button settingButton;
    public Button skillButton;
    public Button fleeButton;
    public GameObject skillTarget;

    public Text waveText;
    public int currentSkillId;

    private void Awake()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            UIManager.Instance.SetCanvas(canvas.transform);
            UIManager.Instance.skillButtonsPanel = GameObject.Find("SkillButtons");
        }
    }
    private void Start()
    {
        // 绑定按钮事件
        settingButton.onClick.AddListener(OnSetting);
        fleeButton.onClick.AddListener(OnFlee);
    }

    private void Update()
    {
        // 刷新 UI
        waveText.text = $"Wave: {BattleManager.Instance.StateMachine.currentWaveIndex}";
        // UpdateHealthUI(BattleManager.Instance.PlayerHealth, BattleManager.Instance.EnemyHealth);
    }

    public void UpdateHealthUI(int playerHealth, int enemyHealth)
    {
        playerHealthText.text = $"Player HP: {playerHealth}";
        enemyHealthText.text = $"Enemy HP: {enemyHealth}";
    }

    private void OnSetting()
    {
        Debug.Log("设置面板");
        UIManager.Instance.ShowPanel("SettingsPanel");
    }

    public void OnUseSkill(int skillId, GameObject target)
    {
        Debug.Log($"Skill used: ID = {skillId}, target = {target}");
    }

    private void OnFlee()
    {
        Debug.Log("逃跑");
        BattleManager.Instance.ClearMonsters();
        BattleManager.Instance.ClearAllies();
        GameManager.Instance.LoadScene("MainScene");
    }
}
