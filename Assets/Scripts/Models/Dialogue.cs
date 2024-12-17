using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public int id;                     // 节点 ID
    public string speaker;             // 说话者
    public string text;                // 对话文本
    public string expression;          // 角色表情
    public string background;          // 背景图片
    public List<Choice> choices;       // 分支选项
    public int? next_id;               // 下一句对话 ID（分支无时使用）
}

[System.Serializable]
public class Choice
{
    public string text;                // 分支文本
    public int next_id;                // 跳转的下一节点 ID
}
