// QuestItem.cs: 挂载到任务预制体上
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private Text questTitleText;
    [SerializeField] private Text questDescriptionText;
    [SerializeField] private Text questProgressText;
    [SerializeField] private Button actionButton;

    private Quest boundQuest;

    // 初始化任务项
    public void Initialize(Quest quest)
    {
        boundQuest = quest;

        // 更新 UI
        questTitleText.text = quest.title;
        questDescriptionText.text = quest.description;
        updateProgressUI();
        updateActionButton();
    }

    private void updateProgressUI()
    {
        questProgressText.text = $"{boundQuest.currentProgress}/{boundQuest.targetAmount}";
    }

    private void updateActionButton()
    {
        if (boundQuest.isCompleted)
        {
            actionButton.interactable = false;
            actionButton.GetComponentInChildren<Text>().text = "Completed";
        }
        else if (boundQuest.currentProgress == 0)
        {
            actionButton.interactable = true;
            actionButton.GetComponentInChildren<Text>().text = "Accept";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(handleAcceptQuest);
        }
        else
        {
            actionButton.interactable = true;
            actionButton.GetComponentInChildren<Text>().text = "Complete";
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(handleCompleteQuest);
        }
    }

    private void handleAcceptQuest()
    {
        QuestManager.Instance.UpdateQuestProgress(boundQuest.questID, 0);
        updateActionButton();
    }

    private void handleCompleteQuest()
    {
        QuestManager.Instance.UpdateQuestProgress(boundQuest.questID, boundQuest.targetAmount);
        updateProgressUI();
        updateActionButton();
    }
}
