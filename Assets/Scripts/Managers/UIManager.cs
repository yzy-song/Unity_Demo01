using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject skillButtonsPanel;

    private Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();
    [SerializeField] private Transform canvasTransform; // 引用场景中的 Canvas

    public void SetCanvas(Transform newCanvasTransform)
    {
        canvasTransform = newCanvasTransform;
    }
    public void RegisterPanel(string panelName, GameObject panel)
    {
        if (!uiPanels.ContainsKey(panelName))
        {
            uiPanels.Add(panelName, panel);
            panel.SetActive(false); // 默认隐藏
        }
    }

    public void ShowPanel(string panelName)
    {
        if (!uiPanels.ContainsKey(panelName))
        {
            var panelPrefab = Resources.Load<GameObject>($"Prefabs/{panelName}");
            if (panelPrefab != null)
            {
                var panelInstance = Instantiate(panelPrefab, canvasTransform); // 设置当前 Canvas 为父对象
                uiPanels[panelName] = panelInstance;
                panelInstance.SetActive(true);
            }
            else
            {
                Debug.LogError($"Panel {panelName} not found in Resources/Prefabs/");
            }
        }
        else
        {
            uiPanels[panelName].SetActive(true);
        }
    }

    public void HidePanel(string panelName)
    {
        if (uiPanels.ContainsKey(panelName))
        {
            uiPanels[panelName].SetActive(false);
        }
    }

    public void ShowSkillButtons(AllyController ally)
    {
        // 激活技能按钮面板
        skillButtonsPanel.SetActive(true);

        // 清理现有的技能按钮
        foreach (Transform child in skillButtonsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // 检查当前状态是否允许点击技能
        bool canInteract = BattleManager.Instance.StateMachine.CurrentState is BattleStatePlayerTurn;
        // 为当前角色生成技能按钮
        foreach (var skill in ally.skills)
        {
            SkillManager.Instance.CreateSkillButton(skill, canInteract, skillButtonsPanel.transform, () =>
            {
                BattleManager.Instance.StateMachine.CurrentState.OnSkillSelected(skill);
            });
        }
    }

}
