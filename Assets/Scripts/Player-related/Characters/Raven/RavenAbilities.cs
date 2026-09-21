using UnityEngine;
using System.Collections;

public class RavenAbilities : MonoBehaviour
{
    public static RavenAbilities Instance;

    [Header("References")]
    public Health playerHealth;
    public Camera playerCamera;
    public Transform firePoint;

    [Header("Utility - Leeching Dodge")]
    public float dodgeCooldown = 4f;
    public float dodgeDuration = 0.25f;
    public float dodgeSpeed = 18f;
    public float bypassHealthCostPercent = 0.10f;
    private float nextDodgeReadyTime = 0f;

    [Header("Ultimate - Liar's Handshake")]
    public float ultimateCooldown = 15f;
    public float ultimateRange = 8f;
    public int ultimateDamage = 40;
    public float stunDuration = 3f;
    public float lifestealWindow = 3f;
    public float lifestealPercent = 0.5f;
    public float lungeSpeed = 30f;
    private float nextUltimateReadyTime = 0f;

    [Header("Signature - Ravensworn")]
    public float signatureCooldown = 60f;
    public float signatureDuration = 10f;
    public float signatureFireRateBonus = 0.5f;
    private float nextSignatureReadyTime = 0f;
    private bool isSignatureActive = false;

    private CharacterController controller;
    private int playerLayer;
    private int enemyLayer;

    public float GetDodgeCooldownFraction() => Mathf.Clamp01((nextDodgeReadyTime - Time.time) / dodgeCooldown);
    public float GetDodgeSecondsRemaining() => Mathf.Max(0f, nextDodgeReadyTime - Time.time);
    public float GetUltimateCooldownFraction() => Mathf.Clamp01((nextUltimateReadyTime - Time.time) / ultimateCooldown);
    public float GetUltimateSecondsRemaining() => Mathf.Max(0f, nextUltimateReadyTime - Time.time);
    public float GetSignatureCooldownFraction() => Mathf.Clamp01((nextSignatureReadyTime - Time.time) / signatureCooldown);
    public float GetSignatureSecondsRemaining() => Mathf.Max(0f, nextSignatureReadyTime - Time.time);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TryDodge();
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextUltimateReadyTime)
        {
            nextUltimateReadyTime = Time.time + ultimateCooldown;
            StartCoroutine(UltimateCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.V) && Time.time >= nextSignatureReadyTime)
        {
            nextSignatureReadyTime = Time.time + signatureCooldown;
            StartCoroutine(SignatureCoroutine());
        }
    }

    void TryDodge()
    {
        bool onCooldown = Time.time < nextDodgeReadyTime;

        if (onCooldown)
        {
            int cost = Mathf.RoundToInt(playerHealth.maxHealth * bypassHealthCostPercent);
            if (playerHealth.currentHealth <= cost) return; // can't afford the bypass
            playerHealth.PayHealthCost(cost);
        }

        nextDodgeReadyTime = Time.time + dodgeCooldown;
        StartCoroutine(DodgeCoroutine());
    }

    IEnumerator DodgeCoroutine()
    {
        playerHealth.isInvulnerable = true;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = transform.right * inputX + transform.forward * inputZ;
        Vector3 dodgeDirection = inputDir.sqrMagnitude > 0.01f ? inputDir.normalized : transform.forward;

        float elapsed = 0f;
        while (elapsed < dodgeDuration)
        {
            controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        playerHealth.isInvulnerable = false;
    }

    IEnumerator UltimateCoroutine()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Physics.Raycast(aimRay, out RaycastHit hit, 100f);

        GameObject targetEnemy = FindNearestEnemyToRay(aimRay);
        if (targetEnemy == null) yield break;

        bool recentlyHit = (Time.time - playerHealth.lastDamageTakenTime) <= lifestealWindow;

        float elapsed = 0f;
        float maxLungeTime = 1f;
        while (elapsed < maxLungeTime && targetEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetEnemy.transform.position);
            if (dist <= 2f) break;

            Vector3 dir = (targetEnemy.transform.position - transform.position).normalized;
            controller.Move(dir * lungeSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (targetEnemy != null)
        {
            Health targetHealth = targetEnemy.GetComponent<Health>();
            if (targetHealth != null)
            {
                int finalDamage = ultimateDamage;
                bool isCrit = false;
                if (PlayerCombatStats.Instance != null)
                    finalDamage = PlayerCombatStats.Instance.RollDamage(ultimateDamage, out isCrit);

                targetHealth.TakeDamage(finalDamage);

                if (DamageNumberSpawner.Instance != null)
                    DamageNumberSpawner.Instance.Spawn(targetEnemy.transform.position, finalDamage, isCrit);

                EnemyAI meleeAI = targetEnemy.GetComponent<EnemyAI>();
                if (meleeAI != null) meleeAI.ApplyStun(stunDuration);
                BossAI bossAI = targetEnemy.GetComponent<BossAI>();
                if (bossAI != null) bossAI.ApplyStun(stunDuration);

                if (recentlyHit)
                {
                    int healAmount = Mathf.RoundToInt(finalDamage * lifestealPercent);
                    playerHealth.currentHealth = Mathf.Min(playerHealth.maxHealth, playerHealth.currentHealth + healAmount);
                    playerHealth.onHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
                }
            }
        }
    }

    GameObject FindNearestEnemyToRay(Ray ray)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject best = null;
        float bestDist = ultimateRange;

        foreach (GameObject enemy in enemies)
        {
            float distToPlayer = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToPlayer > ultimateRange) continue;

            Vector3 toEnemy = (enemy.transform.position - ray.origin).normalized;
            float angle = Vector3.Angle(ray.direction, toEnemy);
            if (angle < 30f && distToPlayer < bestDist)
            {
                bestDist = distToPlayer;
                best = enemy;
            }
        }
        return best;
    }

    IEnumerator SignatureCoroutine()
    {
        isSignatureActive = true;
        yield return new WaitForSeconds(signatureDuration);
        isSignatureActive = false;
    }

    public bool IsSignatureActive() => isSignatureActive;

    public float GetFireRateMultiplier()
    {
        return isSignatureActive ? 1f + signatureFireRateBonus : 1f;
    }
}