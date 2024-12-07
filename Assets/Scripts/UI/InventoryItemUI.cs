using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text quantity;

    public void SetData(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItem.item != null)
        {
            icon.sprite = Resources.Load<Sprite>($"Textures/Items/{inventoryItem.item.iconPath}");
            if (icon.sprite == null)
            {
                Debug.LogWarning($"未找到物品图标: {inventoryItem.item.iconPath}");
            }
            // itemName.text = inventoryItem.item.itemName;
            // quantity.text = $"x{inventoryItem.quantity}";
        }
        else
        {
            Debug.LogWarning("InventoryItem 或其中的 Item 为 null，无法设置数据！");
        }
    }

}
