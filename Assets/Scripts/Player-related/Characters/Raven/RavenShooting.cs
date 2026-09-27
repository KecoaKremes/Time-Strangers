using UnityEngine;
using System.Collections.Generic;

public class RavenShooting : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform firePoint;

    [Header("Primary - Violent Velvet")]
    public int primaryDamage = 15;
    public float primaryFireRate = 0.2f;

    [Header("Secondary - Bloody Rose")]
    public int secondaryDamage = 50;
    public float secondaryCooldown = 6f;

    public float maxRange = 100f;
    public LayerMask hitMask;

    private float nextPrimaryTime;
    private float nextSecondaryTime;
    public Animator animator;

    public float GetPrimaryCooldownFraction() => Mathf.Clamp01((nextPrimaryTime - Time.time) / primaryFireRate);
    public float GetPrimarySecondsRemaining() => Mathf.Max(0f, nextPrimaryTime - Time.time);
    public float GetSecondaryCooldownFraction() => Mathf.Clamp01((nextSecondaryTime - Time.time) / secondaryCooldown);
    public float GetSecondarySecondsRemaining() => Mathf.Max(0f, nextSecondaryTime - Time.time);

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextPrimaryTime)
        {
            FirePrimary();
            float rateMult = RavenAbilities.Instance != null ? RavenAbilities.Instance.GetFireRateMultiplier() : 1f;
            nextPrimaryTime = Time.time + (primaryFireRate / rateMult);
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= nextSecondaryTime)
        {
            FireSecondary();
            nextSecondaryTime = Time.time + secondaryCooldown;
        }
    }

    public Vector3 GetAimPoint()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(aimRay, out RaycastHit hit, maxRange))
            return hit.point;
        return aimRay.origin + aimRay.direction * maxRange;
    }

    void FirePrimary()
    {
        if (animator != null) animator.SetTrigger("FireRifle");
        Vector3 aimPoint = GetAimPoint();
        Vector3 direction = (aimPoint - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, maxRange, hitMask))
        {
            DealDamageToTarget(hit.collider, primaryDamage);
        }
    }

    void FireSecondary()
    {
        if (animator != null) animator.SetTrigger("FireRevolver");
        Vector3 aimPoint = GetAimPoint();
        Vector3 direction = (aimPoint - firePoint.position).normalized;

        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction, maxRange, hitMask);
        List<RaycastHit> sortedHits = new List<RaycastHit>(hits);
        sortedHits.Sort((a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in sortedHits)
        {
            DealDamageToTarget(hit.collider, secondaryDamage);
        }
    }

    void DealDamageToTarget(Collider target, int baseDamage)
    {
        if (!target.CompareTag("Enemy")) return;

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth == null) return;

        int finalDamage = baseDamage;
        bool isCrit = false;

        if (PlayerCombatStats.Instance != null)
            finalDamage = PlayerCombatStats.Instance.RollDamage(baseDamage, out isCrit);

        targetHealth.TakeDamage(finalDamage);

        if (DamageNumberSpawner.Instance != null)
            DamageNumberSpawner.Instance.Spawn(target.transform.position, finalDamage, isCrit);
    }
}