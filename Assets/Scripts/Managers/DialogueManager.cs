using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : Singleton<DialogueManager>
{
    public Text dialogueText;                    // 对话文本显示
    public Text speakerName;                     // 说话者名字
    public Image leftCharacter, rightCharacter; // 左右角色立绘
    public Image background;                    // 背景图片
    public GameObject choicesContainer;         // 分支选项容器
    public GameObject choicePrefab;             // 分支按钮预制体

    private List<Dialogue> dialogues;           // 全部对话数据
    private int currentDialogueId;              // 当前对话节点 ID

    protected override void Awake()
    {
        base.Awake(); // 确保单例初始化逻辑运行
    }

    // 加载 JSON 文件
    void LoadDialogues()
    {
        // 从 Resources 文件夹加载对话文本资源
        TextAsset textAsset = Resources.Load<TextAsset>("Dialogues"); // "Dialogues" 是文件名，无需扩展名
        if (textAsset != null)
        {
            string jsonContent = textAsset.text; // 获取 JSON 内容

            // 使用 JsonHelper 解析 JSON 数组为 Dialogue 对象数组
            Dialogue[] dialogueArray = JsonHelper.FromJson<Dialogue>(jsonContent);

            if (dialogueArray != null)
            {
                dialogues = new List<Dialogue>(dialogueArray); // 转为 List 存储
                Debug.Log("Dialogues loaded successfully from Resources!");
            }
            else
            {
                dialogues = new List<Dialogue>();
                Debug.LogError("Failed to parse Dialogues JSON content!");
            }
        }
        else
        {
            Debug.LogError("Dialogue file not found in Resources!");
        }
    }


    // 开始对话
    public void StartDialogue(int dialogueId)
    {
        currentDialogueId = dialogueId;
        DisplayDialogue(dialogues.Find(d => d.id == dialogueId));
    }

    // 显示对话内容
    private void DisplayDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
        {
            Debug.Log("对话结束");
            return;
        }

        // 更新文本和说话者
        dialogueText.text = "";
        speakerName.text = dialogue.speaker;

        // 动态更新角色立绘和背景
        UpdateCharacter(leftCharacter, dialogue.speaker == "萝莎莱", dialogue.expression);
        UpdateCharacter(rightCharacter, dialogue.speaker != "萝莎莱", dialogue.expression);
        background.sprite = Resources.Load<Sprite>("Backgrounds/" + dialogue.background);

        // 动态显示对话文本
        StartCoroutine(TypeText(dialogue.text));

        // 显示分支选项
        if (dialogue.choices != null && dialogue.choices.Count > 0)
        {
            ShowChoices(dialogue.choices);
        }
        else
        {
            // 如果无分支，自动跳转到下一句
            Invoke("NextDialogue", 2f);
        }
    }

    // 更新角色立绘
    private void UpdateCharacter(Image character, bool isActive, string expression)
    {
        character.gameObject.SetActive(isActive);
        if (isActive)
        {
            character.sprite = Resources.Load<Sprite>("Characters/" + expression);
        }
    }

    // 打字机效果
    private IEnumerator TypeText(string text)
    {
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 显示分支选项
    private void ShowChoices(List<Choice> choices)
    {
        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var choice in choices)
        {
            GameObject button = Instantiate(choicePrefab, choicesContainer.transform);
            button.GetComponentInChildren<Text>().text = choice.text;
            button.GetComponent<Button>().onClick.AddListener(() => StartDialogue(choice.next_id));
        }
    }

    // 下一句对话
    private void NextDialogue()
    {
        Dialogue nextDialogue = dialogues.Find(d => d.id == dialogues.Find(di => di.id == currentDialogueId).next_id);
        DisplayDialogue(nextDialogue);
    }
}
