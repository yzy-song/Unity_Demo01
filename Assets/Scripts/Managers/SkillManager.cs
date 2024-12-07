using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : Singleton<SkillManager>
{
    private Dictionary<int, Skill> skillDictionary = new Dictionary<int, Skill>();
    protected override void Awake()
    {
        Debug.Log("SkillManager Awake");
        base.Awake();
        LoadSkills();
    }
    public void ExecuteSkill(Skill skill, Transform caster, Transform target, System.Action onComplete)
    {
        Debug.Log($"技能 {skill.skillName} 正在释放");

        // 播放特效
        if (skill.effectPrefab != null)
        {
            GameObject effect = Instantiate(skill.effectPrefab, target.position, Quaternion.identity);
            Destroy(effect, 2f); // 销毁特效
        }

        // 播放音效
        if (skill.sound != null)
        {
            AudioSource.PlayClipAtPoint(skill.sound, caster.position);
        }

        // 处理伤害
        if (target.TryGetComponent<MonsterController>(out var enemy))
        {
            enemy.TakeDamage(skill.damage);
        }

        // 技能完成回调
        onComplete?.Invoke();
    }

    // 从配置文件加载技能
    public void LoadSkills()
    {
        TextAsset skillJson = Resources.Load<TextAsset>("Skills");
        SkillList skillConfig = JsonUtility.FromJson<SkillList>(skillJson.text);
        foreach (Skill skill in skillConfig.skills)
        {
            skillDictionary[skill.skillId] = skill;

            // 加载特效和音效
            if (!string.IsNullOrEmpty(skill.effectPrefabPath))
            {
                skill.effectPrefab = Resources.Load<GameObject>(skill.effectPrefabPath);
            }

            if (!string.IsNullOrEmpty(skill.soundPath))
            {
                skill.sound = Resources.Load<AudioClip>(skill.soundPath);
            }
            Debug.Log($"Skill ID: {skill.skillId}, Name: {skill.skillName}");
        }

        Debug.Log("技能数据加载完成");
    }

    // 根据 skillId 获取技能
    public Skill GetSkillById(int skillId)
    {
        if (skillDictionary.TryGetValue(skillId, out var skill))
        {
            return skill;
        }

        Debug.LogError($"未找到技能 ID: {skillId}");
        return null;
    }

    public GameObject CreateSkillButton(Skill skill, bool interactable, Transform parent, UnityEngine.Events.UnityAction onClickAction)
    {
        // 创建一个新的按钮对象
        GameObject button = new GameObject(skill.skillName);
        button.AddComponent<ButtonScaler>();
        button.transform.SetParent(parent, false);

        // 添加 Button 和 Image 组件
        Button buttonComponent = button.AddComponent<Button>();

        Image buttonImage = button.AddComponent<Image>();

        // 从 Resources/Textures 动态加载图片
        Sprite skillIcon = Resources.Load<Sprite>($"Textures/Skills/{skill.icon}");
        if (skillIcon != null)
        {
            buttonImage.sprite = skillIcon;
        }
        else
        {
            Debug.LogWarning($"未找到技能图标: Textures/Skills/{skill.icon}");
        }

        // 添加文字
        // GameObject textObject = new GameObject("Text");
        // textObject.transform.SetParent(button.transform, false);
        // Text textComponent = textObject.AddComponent<Text>();
        // textComponent.text = skill.skillName;
        // textComponent.alignment = TextAnchor.MiddleCenter;
        // textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        // textComponent.color = Color.black;

        // 自动调整按钮的尺寸
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(100, 100); // 设置按钮尺寸

        // RectTransform textRect = textObject.GetComponent<RectTransform>();
        // textRect.sizeDelta = buttonRect.sizeDelta;

        buttonComponent.interactable = interactable;

        if (interactable)
        {
            // 设置按钮点击事件
            buttonComponent.onClick.AddListener(onClickAction);
        }

        return button;
    }

    // public GameObject CreateSkillButton(Skill skill, Transform parent, UnityEngine.Events.UnityAction onClickAction)
    // {
    //     GameObject button = Instantiate(Resources.Load<GameObject>("Prefabs/SkillButton"), parent);

    //     // 设置按钮文本
    //     var buttonText = button.GetComponentInChildren<UnityEngine.UI.Text>();
    //     if (buttonText != null)
    //     {
    //         buttonText.text = skill.skillName;
    //     }

    //     // 设置点击事件
    //     var buttonComponent = button.GetComponent<UnityEngine.UI.Button>();
    //     if (buttonComponent != null)
    //     {
    //         buttonComponent.onClick.AddListener(onClickAction);
    //     }

    //     return button;
    // }

}
