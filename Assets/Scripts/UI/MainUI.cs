using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button noticeButton;
    public Button settingsButton;
    public Button chatButton;
    public Button bagButton;
    public Button shopButton;
    public Button questButton;
    public Button battleButton;

    public Text userTxt;

    private void Start()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            UIManager.Instance.SetCanvas(canvas.transform);
        }
        // 绑定按钮事件
        noticeButton.onClick.AddListener(OnNotice);
        settingsButton.onClick.AddListener(OnOpenSettings);
        questButton.onClick.AddListener(OnOpenQuest);
        chatButton.onClick.AddListener(OnChat);

        battleButton.onClick.AddListener(OnBattle);

        bagButton.onClick.AddListener(OnOpenBag);

        userTxt.text = LoginManager.LoggedInUsername;

    }

    private void OnHotUpdate()
    {
        Debug.Log("检测热更新");
    }

    private void OnOpenBag()
    {
        Debug.Log("打开背包界面");
        UIManager.Instance.ShowPanel("InventoryPanel");
    }

    private void OnChat()
    {
        Debug.Log("打开聊天界面");
    }

    private void OnNotice()
    {
        Debug.Log("打开公告界面");
    }

    private void OnOpenSettings()
    {
        UIManager.Instance.ShowPanel("SettingsPanel");
    }
    private void OnOpenQuest()
    {
        UIManager.Instance.ShowPanel("QuestPanel");
    }

    private void OnQuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }

    private void OnBattle()
    {
        GameManager.Instance.LoadScene("BattleScene");
    }
}
