using UnityEngine;
using UnityEngine.Events;

public class PlayerProgression : MonoBehaviour
{
    public static PlayerProgression Instance;

    [Header("Currency")]
    public int chronoCoins = 0;
    public UnityEvent<int> onCoinsChanged;

    [Header("Leveling")]
    public int level = 1;
    public int currentXP = 0;
    public int baseXPRequirement = 100;
    public float xpCurveMultiplier = 1.2f;
    public UnityEvent<int, int, int> onXPChanged; // level, currentXP, xpToNextLevel

    [Header("Level-up bonuses")]
    public int healthPerLevel = 10;
    public float damagePerLevel = 0.05f; // +5% damage per level

    public Health playerHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        onXPChanged?.Invoke(level, currentXP, GetXPRequiredForLevel(level));
        onCoinsChanged?.Invoke(chronoCoins);
    }

    public void AddCoins(int amount)
    {
        chronoCoins += amount;
        onCoinsChanged?.Invoke(chronoCoins);
    }

    // Called later by item boxes when spending currency
    public bool SpendCoins(int amount)
    {
        if (chronoCoins < amount) return false;
        chronoCoins -= amount;
        onCoinsChanged?.Invoke(chronoCoins);
        return true;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        int xpNeeded = GetXPRequiredForLevel(level);
        while (currentXP >= xpNeeded)
        {
            currentXP -= xpNeeded;
            LevelUp();
            xpNeeded = GetXPRequiredForLevel(level);
        }

        onXPChanged?.Invoke(level, currentXP, xpNeeded);
    }

    int GetXPRequiredForLevel(int lvl)
    {
        return Mathf.RoundToInt(baseXPRequirement * Mathf.Pow(lvl, xpCurveMultiplier));
    }

    void LevelUp()
    {
        level++;
        playerHealth.maxHealth += healthPerLevel;
        playerHealth.currentHealth = playerHealth.maxHealth; // full heal on level up
        playerHealth.onHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
    }

    public float GetLevelDamageMultiplier()
    {
        return 1f + (level - 1) * damagePerLevel;
    }
}