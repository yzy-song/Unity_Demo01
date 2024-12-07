using UnityEngine;

public class HorizontalLayout2D : MonoBehaviour
{
    public float spacing = 4.0f; // 每个对象之间的水平间距
    public Vector2 startOffset = Vector2.zero; // 起始偏移量

    public void UpdateLayout()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // 计算总宽度（包括间距）
        float totalWidth = (childCount - 1) * spacing;

        // 动态调整父节点的位置，使其居中
        Vector3 parentOffset = new Vector3(-totalWidth / 2, transform.position.y, 0);
        transform.localPosition = parentOffset;

        // 遍历子对象，调整子对象位置
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                child.localPosition = new Vector2(i * spacing + startOffset.x, startOffset.y);
            }
        }
    }

    private void Start()
    {
        UpdateLayout();
    }

    private void OnTransformChildrenChanged()
    {
        UpdateLayout();
    }
}
