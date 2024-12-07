using UnityEngine;

[ExecuteAlways]
public class RectTransformBorder : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) return;

        // 获取 RectTransform 的四个顶点
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // 设置 Gizmos 的颜色
        Gizmos.color = Color.green;

        // 绘制矩形边框
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}
