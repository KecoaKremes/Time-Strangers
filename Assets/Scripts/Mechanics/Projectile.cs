using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    public float lifetime = 3f;
    public int damage = 10;
    public string targetTag = "Enemy";

    public bool isPlayerOwned = true;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Health targetHealth = other.GetComponent<Health>();

            if (targetHealth != null)
            {
                int finalDamage = damage;
                bool isCrit = false;

                if (isPlayerOwned && PlayerCombatStats.Instance != null)
                    finalDamage = PlayerCombatStats.Instance.RollDamage(
                        damage,
                        out isCrit
                    );

                targetHealth.TakeDamage(finalDamage);

                if (DamageNumberSpawner.Instance != null)
                {
                    DamageNumberSpawner.Instance.Spawn(
                        other.transform.position,
                        finalDamage,
                        isCrit
                    );
                }
                int spearheadStacks = PlayerInventory.Instance.GetStackCount(ItemID.BrokenSpearhead);
    if (spearheadStacks > 0)
    {
        float chance = Mathf.Min(0.05f + 0.05f * (spearheadStacks - 1), 0.10f);
        if (Random.value < chance)
        {
            bool isBoss = other.GetComponent<BossAI>() != null;
            int bleedTotal = Mathf.RoundToInt(targetHealth.maxHealth * (isBoss ? 0.05f : 0.10f));
            BleedEffect bleed = other.gameObject.AddComponent<BleedEffect>();
            bleed.Apply(targetHealth, bleedTotal, 10f, 10);
        }
    }

    int rabbitStacks = PlayerInventory.Instance.GetStackCount(ItemID.RabbitsFoot);
    if (isCrit && rabbitStacks > 0)
    {
        Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.currentHealth = Mathf.Min(playerHealth.maxHealth, playerHealth.currentHealth + finalDamage * rabbitStacks);
        playerHealth.onHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
    }
            }

            Destroy(gameObject);
        }
    }
}