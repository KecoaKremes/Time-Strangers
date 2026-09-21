using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemData> commonItems;
    public List<ItemData> uncommonItems;
    public List<ItemData> legendaryItems;

    void Awake() { Instance = this; }

    public ItemData GetRandomItemOfRarity(ItemRarity rarity)
    {
        List<ItemData> pool = rarity switch
        {
            ItemRarity.Common => commonItems,
            ItemRarity.Uncommon => uncommonItems,
            ItemRarity.Legendary => legendaryItems,
            _ => commonItems
        };

        if (pool == null || pool.Count == 0) return null;
        return pool[Random.Range(0, pool.Count)];
    }

    public List<ItemData> chronosItems;

    public ItemData GetRandomChronosItem()
    {
        if (chronosItems == null || chronosItems.Count == 0) return null;
        return chronosItems[Random.Range(0, chronosItems.Count)];
    }
}