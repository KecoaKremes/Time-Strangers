using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyReward : MonoBehaviour
{
    public int coinReward = 5;
    public int xpReward = 10;

    void Start()
    {
        GetComponent<Health>().onDeath.AddListener(GrantReward);
    }

    void GrantReward()
{
    if (PlayerProgression.Instance != null)
    {
        PlayerProgression.Instance.AddCoins(coinReward);
        PlayerProgression.Instance.AddXP(xpReward);
    }

    int fruitStacks = PlayerInventory.Instance.GetStackCount(ItemID.FruitOffering);
    if (fruitStacks > 0 && Random.value < 0.01f * fruitStacks)
    {
        ItemRarity roll = Random.value < 0.70f ? ItemRarity.Common : (Random.value < 0.95f ? ItemRarity.Uncommon : ItemRarity.Legendary);
        ItemData dropped = ItemDatabase.Instance.GetRandomItemOfRarity(roll);
        if (dropped != null)
            ChestItemLauncher.Instance.LaunchItem(transform.position, dropped, transform);
    }
}
}