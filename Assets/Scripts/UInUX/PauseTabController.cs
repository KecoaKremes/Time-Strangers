using UnityEngine;

public class PauseTabController : MonoBehaviour
{
    public GameObject itemsPanel;
    public GameObject abilitiesPanel;
    public InventoryPanelUI itemsUI;
    public AbilityListUI abilitiesUI;

    public void ShowItems()
    {
        itemsPanel.SetActive(true);
        abilitiesPanel.SetActive(false);
        itemsUI.Refresh();
    }

    public void ShowAbilities()
    {
        itemsPanel.SetActive(false);
        abilitiesPanel.SetActive(true);
        abilitiesUI.Refresh();
    }
}