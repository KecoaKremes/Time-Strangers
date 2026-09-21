using UnityEngine;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI stackText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(ItemData item, int stackCount)
    {
        if (item == null) return;
        panel.SetActive(true);
        nameText.text = item.itemName;
        rarityText.text = item.rarity.ToString();
        descriptionText.text = item.description;
        stackText.text = "x" + stackCount;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}