using UnityEngine;

public class ItemEffectSystem : MonoBehaviour
{
    public static ItemEffectSystem Instance;
    public Health playerHealth;
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;

    void Awake()
{
    Instance = this;

    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player != null)
    {
        playerHealth = player.GetComponent<Health>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShooting = player.GetComponent<PlayerShooting>();
    }
}

    public void ApplyStackEffect(ItemData item, int newStackCount)
    {
        switch (item.id)
        {
            case ItemID.HardBoiledEgg:
                playerHealth.maxHealth += 25;
                playerHealth.currentHealth += 25;
                playerHealth.onHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
                break;
            case ItemID.WornSneakers:
                playerMovement.walkSpeed *= 1.10f;
                playerMovement.sprintSpeed *= 1.10f;
                break;
            case ItemID.GunslingersHolster:
                playerShooting.fireRate *= 0.90f;
                break;
            case ItemID.BrokenWing:
                playerMovement.maxJumps += 1;
                break;
            case ItemID.HourHand:
                AnomalyManager.Instance.AddTime(120f);
                break;
            case ItemID.GlassShard:
                AnomalyManager.Instance.ReduceTime(30f);
                break;
            case ItemID.SwirlyGlasses:
                AnomalyManager.Instance.ReduceTime(60f);
                break;
            case ItemID.BadgersHoney:
                playerHealth.regenPerSecond += 5f;
                break;
            case ItemID.RavenFeather:
                // checked directly in PlayerMovement, no stat change needed here
                break;
            case ItemID.CrackedTimeWatch:
                AnomalyManager.Instance.ReduceTime(180f);
                break;    
            case ItemID.MagicKarp:
            playerHealth.maxHealth = Mathf.RoundToInt(playerHealth.maxHealth * 0.25f);
            playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth, playerHealth.maxHealth);
            AnomalyManager.Instance.ReduceTime(60f);
            break;    
        }
    }

    public float GetSecondHandMultiplier()
{
    int stacks = PlayerInventory.Instance.GetStackCount(ItemID.SecondHand);
    return Mathf.Pow(1.10f, stacks);
}

public float GetSwirlyGlassesMultiplier()
{
    int stacks = PlayerInventory.Instance.GetStackCount(ItemID.SwirlyGlasses);
    return stacks > 0 ? Mathf.Pow(2f, stacks) : 1f; // 100% extra = x2, compounds per stack
}

public bool HasRavenFeather() => PlayerInventory.Instance.GetStackCount(ItemID.RavenFeather) > 0;

public float GetMinuteHandReduction()
{
    int stacks = PlayerInventory.Instance.GetStackCount(ItemID.MinuteHand);
    return Mathf.Min(0.05f * stacks, 0.90f); // caps at 18 stacks
}

public float GetUShapedBarrelDamageBonus(int maxHealth)
{
    return PlayerInventory.Instance.GetStackCount(ItemID.UShapedBarrel) > 0 ? maxHealth / 100f : 0f;
}

// Fox Shrine: counts enemies within/beyond range live, called every frame by movement/shooting
public float GetFoxShrineAttackSpeedMultiplier(Vector3 playerPos)
{
    int stacks = PlayerInventory.Instance.GetStackCount(ItemID.FoxShrine);
    if (stacks == 0) return 1f;
    int maxProcs = 3 + (stacks - 1);
    int closeEnemies = Mathf.Min(CountEnemiesWithinRadius(playerPos, 10f), maxProcs);
    return 1f + (0.07f * closeEnemies);
}

public float GetFoxShrineMoveSpeedMultiplier(Vector3 playerPos)
{
    int stacks = PlayerInventory.Instance.GetStackCount(ItemID.FoxShrine);
    if (stacks == 0) return 1f;
    int maxProcs = 3 + (stacks - 1);
    int farEnemies = Mathf.Min(CountEnemiesFarFromRadius(playerPos, 10f), maxProcs);
    return 1f + (0.07f * farEnemies);
}

int CountEnemiesWithinRadius(Vector3 pos, float radius)
{
    Collider[] hits = Physics.OverlapSphere(pos, radius);
    int count = 0;
    foreach (var h in hits) if (h.CompareTag("Enemy")) count++;
    return count;
}

int CountEnemiesFarFromRadius(Vector3 pos, float radius)
{
    GameObject[] all = GameObject.FindGameObjectsWithTag("Enemy");
    int count = 0;
    foreach (var e in all) if (Vector3.Distance(pos, e.transform.position) > radius) count++;
    return count;
}






//gap



    public float GetDamageDealtMultiplier()
{
    float bonus = 0f;
    bonus += 0.25f * PlayerInventory.Instance.GetStackCount(ItemID.GlassShard);

    int stopwatchStacks = PlayerInventory.Instance.GetStackCount(ItemID.Stopwatch);
    if (stopwatchStacks > 0 && (playerMovement.IsSprinting() || !playerMovement.IsGrounded()))
        bonus += 0.10f * stopwatchStacks;

    int pristineStacks = PlayerInventory.Instance.GetStackCount(ItemID.PristineTimeWatch);
    if (pristineStacks > 0)
    {
        float minutesElapsed = (AnomalyManager.Instance.stageDuration - AnomalyManager.Instance.GetTimeRemaining()) / 60f;
        bonus += 0.03f * minutesElapsed * pristineStacks;
    }

    bonus += GetUShapedBarrelDamageBonus(playerHealth.maxHealth);

    float multiplier = (1f + bonus) * GetSecondHandMultiplier();
    return multiplier;
}

    public float GetDamageTakenMultiplier()
{
    float increase = 1f + (0.50f * PlayerInventory.Instance.GetStackCount(ItemID.GlassShard));
    float reduction = 1f - GetMinuteHandReduction();
    return increase * reduction;
}
}