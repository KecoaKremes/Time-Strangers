using UnityEngine;

public class PlayerCombatStats : MonoBehaviour
{
    public static PlayerCombatStats Instance;

    public float baseCritChance = 0.05f;
    public float critDamageMultiplier = 2f;

    void Awake() { Instance = this; }

    public float GetCritChance()
    {
        float bonus = PlayerInventory.Instance != null
            ? PlayerInventory.Instance.GetStackCount(ItemID.BrokenTimeWatch) * 0.07f
            : 0f;
        return Mathf.Clamp01(baseCritChance + bonus);
    }

    public int RollDamage(int baseDamage, out bool isCrit)
{
    float multiplier = 1f;
    if (PlayerAbilities.Instance != null) multiplier *= PlayerAbilities.Instance.GetDamageMultiplier();
    if (PlayerProgression.Instance != null) multiplier *= PlayerProgression.Instance.GetLevelDamageMultiplier();
    if (ItemEffectSystem.Instance != null) multiplier *= ItemEffectSystem.Instance.GetDamageDealtMultiplier();
    if (RavenPassive.Instance != null) multiplier *= RavenPassive.Instance.GetDamageMultiplier();

    bool inverted = PlayerInventory.Instance != null && PlayerInventory.Instance.GetStackCount(ItemID.SwirlyGlasses) > 0;
    bool forcedCrit = RavenAbilities.Instance != null && RavenAbilities.Instance.IsSignatureActive();
    bool rolledCrit = forcedCrit || Random.value < GetCritChance();

    if (inverted)
    {
        isCrit = false;
        return rolledCrit ? 1 : Mathf.RoundToInt(baseDamage * multiplier * 2f);
    }

    isCrit = rolledCrit;
    if (rolledCrit) multiplier *= critDamageMultiplier;
    return Mathf.RoundToInt(baseDamage * multiplier);
}
}