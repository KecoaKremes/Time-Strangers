using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Camera playerCamera;
    public float fireRate = 0.25f;
    public float maxAimDistance = 100f;

    [HideInInspector] public bool isShotgunMode = false;
    public int shotgunPelletCount = 6;
    public float shotgunSpreadAngle = 12f;

    private float nextFireTime;

    // CHANGED: removed the ItemEffectSystem calculation from here
    float attackSpeedMult = 1f;

    public bool canShoot = true;

    void Update()
    {
        // ADDED: calculate attack speed here instead
        attackSpeedMult = ItemEffectSystem.Instance != null
            ? ItemEffectSystem.Instance.GetFoxShrineAttackSpeedMultiplier(transform.position)
            : 1f;

        if (canShoot && Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (fireRate / attackSpeedMult);
        }
    }

    public Vector3 GetAimPoint()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(aimRay, out RaycastHit hit, maxAimDistance))
        {
            return hit.point;
        }
        return aimRay.origin + aimRay.direction * maxAimDistance;
    }

    void Shoot()
    {
        Vector3 aimPoint = GetAimPoint();

        if (isShotgunMode)
        {
            FireShotgunSpread(aimPoint);
        }
        else
        {
            FireSingleProjectile(aimPoint);
        }
    }

    public PlayerAbilities playerAbilities; // drag Player in

    void FireSingleProjectile(Vector3 aimPoint)
    {
        Vector3 direction = (aimPoint - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        ApplyDamageMultiplier(proj);
    }

    void FireShotgunSpread(Vector3 aimPoint)
    {
        Vector3 baseDirection = (aimPoint - firePoint.position).normalized;
        for (int i = 0; i < shotgunPelletCount; i++)
        {
            Vector3 spreadDirection = Quaternion.Euler(
                Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle),
                Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle), 0f) * baseDirection;
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(spreadDirection));
            ApplyDamageMultiplier(proj);
        }
    }

    void ApplyDamageMultiplier(GameObject proj)
    {
        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            float multiplier = 1f;
            if (playerAbilities != null) multiplier *= playerAbilities.GetDamageMultiplier();
            if (PlayerProgression.Instance != null) multiplier *= PlayerProgression.Instance.GetLevelDamageMultiplier();

            projScript.damage = Mathf.RoundToInt(projScript.damage * multiplier);
        }
    }

    public float GetFireCooldownFraction()
    {
        return Mathf.Clamp01((nextFireTime - Time.time) / fireRate);
    }

    public float GetFireSecondsRemaining()
    {
        return Mathf.Max(0f, nextFireTime - Time.time);
    }
}