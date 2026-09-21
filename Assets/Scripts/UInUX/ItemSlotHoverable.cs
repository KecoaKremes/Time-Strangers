using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image holdProgressFill; // assign on the slot prefab
    public float holdDuration = 0.6f;

    private ItemData item;
    private int stackCount;
    private bool isHolding = false;
    private float holdTimer = 0f;

    public void Setup(ItemData assignedItem, int count)
    {
        item = assignedItem;
        stackCount = count;
        if (holdProgressFill != null) holdProgressFill.fillAmount = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        ItemTooltipUI.Instance.Show(item, stackCount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
        CancelHold();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelHold();
    }

    void Update()
    {
        if (!isHolding) return;

        holdTimer += Time.unscaledDeltaTime;
        if (holdProgressFill != null) holdProgressFill.fillAmount = holdTimer / holdDuration;

        if (holdTimer >= holdDuration)
        {
            PlayerInventory.Instance.RemoveItem(item);
            isHolding = false;
        }
    }

    void CancelHold()
    {
        isHolding = false;
        holdTimer = 0f;
        if (holdProgressFill != null) holdProgressFill.fillAmount = 0f;
    }
}