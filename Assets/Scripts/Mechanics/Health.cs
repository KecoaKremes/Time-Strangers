using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // UnityEvents let us drag-and-drop responses in the Inspector too,
    // but here we'll mainly subscribe via code.
    public UnityEvent<int, int> onHealthChanged; // (current, max)
    public UnityEvent onDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Fire once at start so UI initializes to the correct value immediately
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
{

    if (isInvulnerable) return;
    lastDamageTakenTime = Time.time;
    if (CompareTag("Player") && ItemEffectSystem.Instance != null)
        amount = Mathf.RoundToInt(amount * ItemEffectSystem.Instance.GetDamageTakenMultiplier());
    currentHealth -= amount;
    currentHealth = Mathf.Max(currentHealth, 0);

    bool isDead = currentHealth <= 0;

    onHealthChanged?.Invoke(currentHealth, maxHealth);

    if (isDead)
    {
        Die();
    }
}

    void Die()
    {
        onDeath?.Invoke();
        // Enemies still get destroyed; Player handles its own death differently (see PlayerHealthHandler)
        if (!CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public float regenPerSecond = 5f;
public bool canRegenerate = true;
public bool isInvulnerable = false;

private float regenAccumulator = 0f;

void Update()
{
    if (canRegenerate && currentHealth > 0 && currentHealth < maxHealth)
    {
        float regenMult = regenMultiplierProvider != null ? regenMultiplierProvider() : 1f;
        regenAccumulator += regenPerSecond * regenMult * Time.deltaTime;

        if (regenAccumulator >= 1f)
        {
            int wholeAmount = Mathf.FloorToInt(regenAccumulator);
            currentHealth = Mathf.Min(currentHealth + wholeAmount, maxHealth);
            regenAccumulator -= wholeAmount;
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}

public void TakeNonLethalDamage(int amount)
{
    if (isInvulnerable) return;
    currentHealth = Mathf.Max(1, currentHealth - amount);
    onHealthChanged?.Invoke(currentHealth, maxHealth);
}

public System.Func<float> regenMultiplierProvider;
public float lastDamageTakenTime = -999f;

public void PayHealthCost(int amount)
{
    currentHealth = Mathf.Max(1, currentHealth - amount); // can't kill yourself paying a cost
    onHealthChanged?.Invoke(currentHealth, maxHealth);
}

}