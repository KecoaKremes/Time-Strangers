using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private Dictionary<ItemData, int> itemStacks = new Dictionary<ItemData, int>();
    public UnityEvent onInventoryChanged;

    void Awake() { Instance = this; }

    public void AddItem(ItemData item)
    {
        itemStacks[item] = itemStacks.ContainsKey(item) ? itemStacks[item] + 1 : 1;
        ItemEffectSystem.Instance.ApplyStackEffect(item, itemStacks[item]);
        onInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        if (itemStacks.ContainsKey(item))
        {
            itemStacks.Remove(item);
            onInventoryChanged?.Invoke();
        }
    }

    public int GetStackCount(ItemID id)
    {
        foreach (var kvp in itemStacks)
            if (kvp.Key.id == id) return kvp.Value;
        return 0;
    }

    public Dictionary<ItemData, int> GetAllStacks() => itemStacks;
}