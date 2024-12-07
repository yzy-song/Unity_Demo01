using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillTransform;  // 血量填充部分的 Transform
    public SpriteRenderer fillSprite; // 血条的 SpriteRenderer（用于颜色动态变化）
    public Transform healthBarTransform; // 整个血条的 Transform（用于定位）
    public TextMeshPro healthText;

    private float maxHealth = 100f;  // 最大血量
    private float currentHealth;     // 当前血量

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void Initialize(float maxHP)
    {
        maxHealth = maxHP;
        currentHealth = maxHealth;
        // 初始时设置为满血
        if (fillTransform != null)
        {
            fillTransform.localScale = new Vector3(currentHealth / maxHealth, 1, 1); // 只缩放 X 轴
        }
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 防止血量溢出
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 防止血量溢出
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // 根据血量百分比缩放填充部分
        float healthPercent = currentHealth / maxHealth;
        if (fillTransform != null)
        {
            fillTransform.localScale = new Vector3(healthPercent, 1, 1); // 只缩放 X 轴
        }

        // 动态改变血条颜色（绿色到红色）
        if (fillSprite != null)
        {
            fillSprite.color = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }

    // private void LateUpdate()
    // {
    //     // 血条始终面向摄像机
    //     if (Camera.main != null && healthBarTransform != null)
    //     {
    //         healthBarTransform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    //     }
    // }
}
