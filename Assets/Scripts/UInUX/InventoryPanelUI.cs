using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanelUI : MonoBehaviour
{
    public static InventoryPanelUI Instance;
    public Transform slotParent;
    public GameObject slotPrefab;

    void Awake() { Instance = this; }

    public void Refresh()
    {
        foreach (Transform child in slotParent) Destroy(child.gameObject);

        foreach (var kvp in PlayerInventory.Instance.GetAllStacks())
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);

            Image img = slot.GetComponent<Image>();
            if (kvp.Key.icon != null) { img.sprite = kvp.Key.icon; img.color = Color.white; }
            else img.color = kvp.Key.placeholderColor;

            slot.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().text = kvp.Key.itemName;
            
            slot.transform.Find("StackCount").GetComponent<TextMeshProUGUI>().text = "x" + kvp.Value;

            ItemSlotHoverable hover = slot.GetComponent<ItemSlotHoverable>();
            if (hover == null) hover = slot.AddComponent<ItemSlotHoverable>();
            Transform fillTransform = slot.transform.Find("HoldProgressFill");
            if (fillTransform != null) hover.holdProgressFill = fillTransform.GetComponent<Image>();
            hover.Setup(kvp.Key, kvp.Value);
        }
    }
}