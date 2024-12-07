using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    public static event System.Action<GameObject> OnObjectClicked;

    public static void Trigger(GameObject clickedObject)
    {
        OnObjectClicked?.Invoke(clickedObject);
    }
}
public class ObjectClickHandler : MonoBehaviour
{
    private bool isAnimating = false;  // 是否正在播放动画
    private Vector3 originalScale;    // 图片的原始缩放
    private Vector3 targetScale;      // 动画的目标缩放
    private float animationTime = 0.1f;  // 动画时长
    private float elapsedTime = 0f;     // 当前动画进度时间

    public static event System.Action<GameObject> OnObjectClicked;


    void Start()
    {
        // 记录图片的原始缩放
        originalScale = transform.localScale;
        targetScale = originalScale * 0.9f;  // 点击时缩小 10%
    }
    // public void OnObjectClicked()
    // {
    //     Debug.Log($"Object {gameObject.name} clicked!");
    //     // 在这里处理按钮点击逻辑

    // }

    private void OnMouseDown()
    {
        if (!isAnimating)
        {
            // 开始动画
            StartCoroutine(PlayClickAnimation());
        }
        ClickEvent.Trigger(gameObject);
    }

    private System.Collections.IEnumerator PlayClickAnimation()
    {
        isAnimating = true;
        elapsedTime = 0f;

        // 缩小动画
        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // 重置时间
        elapsedTime = 0f;

        // 放大回原始尺寸动画
        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        // 动画结束
        transform.localScale = originalScale;
        isAnimating = false;
    }
}
