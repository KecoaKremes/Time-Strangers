using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityListUI : MonoBehaviour
{
    public Transform rowParent;
    public GameObject rowPrefab;

    public void Refresh()
    {
        foreach (Transform child in rowParent) Destroy(child.gameObject);

        ICharacterAbilityUI current = CharacterAbilityRegistry.Current;
        if (current == null) return;

        foreach (AbilityInfo info in current.GetAbilityInfos())
        {
            GameObject row = Instantiate(rowPrefab, rowParent);

            row.transform.Find("KeyText").GetComponent<TextMeshProUGUI>().text = info.keyLabel;
            row.transform.Find("SlotNameText").GetComponent<TextMeshProUGUI>().text = info.slotName;
            row.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = info.description;

            Image icon = row.transform.Find("IconImage").GetComponent<Image>();
            if (info.icon != null) { icon.sprite = info.icon; icon.enabled = true; }
            else icon.enabled = false;
        }
    }
}