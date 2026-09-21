using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Transform slotParent;
    public GameObject slotPrefab;

    void Start()
    {
        PlayerInventory.Instance.onInventoryChanged.AddListener(Refresh);
    }

    void Refresh()
    {
        
        foreach (Transform child in slotParent) Destroy(child.gameObject);

        foreach (var kvp in PlayerInventory.Instance.GetAllStacks())
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponentInChildren<Image>().color = kvp.Key.placeholderColor;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = "x" + kvp.Value;
            Image img = slot.GetComponentInChildren<Image>();
            if (kvp.Key.icon != null)
        {
            img.sprite = kvp.Key.icon;
            img.color = Color.white;
        }
        else
        {
            img.color = kvp.Key.placeholderColor;
        }
        }
    }
}