using UnityEngine;

public class RavenPassive : MonoBehaviour
{
    public static RavenPassive Instance;

    [Header("Distance thresholds")]
    public float closeDistance = 3f;   // at or below this = max damage bonus
    public float farDistance = 20f;    // at or above this = max heal bonus

    [Header("Bonus caps")]
    public float maxHealMultiplier = 2f;   // up to double healing
    public float maxDamageBonus = 1f;      // up to +100% damage

    private float cachedNearestDistance = 999f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Health health = GetComponent<Health>();
        health.regenMultiplierProvider = GetHealMultiplier;
    }

    void Update()
    {
        cachedNearestDistance = FindNearestEnemyDistance();
    }

    float FindNearestEnemyDistance()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearest = 999f;
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < nearest) nearest = dist;
        }
        return nearest;
    }

    public float GetHealMultiplier()
    {
        float t = Mathf.InverseLerp(closeDistance, farDistance, cachedNearestDistance);
        return Mathf.Lerp(1f, maxHealMultiplier, t);
    }

    public float GetDamageMultiplier()
    {
        float t = Mathf.InverseLerp(farDistance, closeDistance, cachedNearestDistance);
        return 1f + Mathf.Lerp(0f, maxDamageBonus, t);
    }
}