using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerAbilities : MonoBehaviour
{
    [Header("References")]
    public PlayerShooting playerShooting;
    public Health playerHealth;
    public Transform firePoint;
    public GameObject aoeProjectilePrefab;

    [Header("M2 - AoE Arc")]
    public float aoeCooldown = 6f;
    public float aoeArcHeight = 3f;
    private float nextAoEReadyTime = 0f;

    [Header("CTRL - Dodge/Phase")]
    public float dodgeCooldown = 4f;
    public float dodgeDuration = 0.25f;
    public float dodgeSpeed = 50f;
    private float nextDodgeReadyTime = 0f;

    [Header("R - Shotgun Mode")]
    public float ultimateCooldown = 20f;
    public float ultimateDuration = 10f;
    private float nextUltimateReadyTime = 0f;


    private CharacterController controller;
    private int playerLayer;
    private int enemyLayer;

    public static PlayerAbilities Instance;
    void Start()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && Time.time >= nextAoEReadyTime)
        {
            nextAoEReadyTime = Time.time + aoeCooldown;
            CastAoE();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Time.time >= nextDodgeReadyTime)
        {
            nextDodgeReadyTime = Time.time + dodgeCooldown;
            StartCoroutine(DodgeCoroutine());
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

    // ---------------- M2: AoE Arc Shot ----------------

    void CastAoE()
{
    Vector3 aimPoint = playerShooting.GetAimPoint();
    GameObject proj = Instantiate(aoeProjectilePrefab, firePoint.position, Quaternion.identity);
    AoEProjectile aoeScript = proj.GetComponent<AoEProjectile>();
    aoeScript.damage = Mathf.RoundToInt(aoeScript.damage * GetDamageMultiplier());
    Vector3 launchVelocity = CalculateArcVelocity(firePoint.position, aimPoint, aoeArcHeight);
    aoeScript.Launch(launchVelocity);
    float multiplier = GetDamageMultiplier() * (PlayerProgression.Instance != null ? PlayerProgression.Instance.GetLevelDamageMultiplier() : 1f);
    aoeScript.damage = Mathf.RoundToInt(aoeScript.damage * multiplier);
}

    // Standard two-stage projectile motion: rises to arcHeight, then falls to target height.
    Vector3 CalculateArcVelocity(Vector3 origin, Vector3 target, float arcHeight)
    {
        float gravity = Physics.gravity.y; // negative value, e.g. -9.81
        float displacementY = target.y - origin.y;
        Vector3 displacementXZ = new Vector3(target.x - origin.x, 0f, target.z - origin.z);

        float timeUp = Mathf.Sqrt(-2f * arcHeight / gravity);
        float timeDown = Mathf.Sqrt(2f * Mathf.Max(0f, arcHeight - displacementY) / -gravity);
        float totalTime = timeUp + timeDown;

        Vector3 velocityXZ = displacementXZ / totalTime;
        float velocityY = Mathf.Sqrt(-2f * gravity * arcHeight);

        return velocityXZ + Vector3.up * velocityY;
    }

    // ---------------- CTRL: Dodge / Phase ----------------

    IEnumerator DodgeCoroutine()
{
    playerHealth.isInvulnerable = true;
    playerShooting.canShoot = false;
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
    playerShooting.canShoot = true;
}

    // ---------------- R: Shotgun Mode ----------------

    IEnumerator UltimateCoroutine()
    {
        playerShooting.isShotgunMode = true;
        yield return new WaitForSeconds(ultimateDuration);
        playerShooting.isShotgunMode = false;
    }

    public float GetAoECooldownFraction()
{
    return Mathf.Clamp01((nextAoEReadyTime - Time.time) / aoeCooldown);
}

public float GetDodgeCooldownFraction()
{
    return Mathf.Clamp01((nextDodgeReadyTime - Time.time) / dodgeCooldown);
}

public float GetUltimateCooldownFraction()
{
    return Mathf.Clamp01((nextUltimateReadyTime - Time.time) / ultimateCooldown);
}

public float GetAoESecondsRemaining()
{
    return Mathf.Max(0f, nextAoEReadyTime - Time.time);
}

public float GetDodgeSecondsRemaining()
{
    return Mathf.Max(0f, nextDodgeReadyTime - Time.time);
}

public float GetUltimateSecondsRemaining()
{
    return Mathf.Max(0f, nextUltimateReadyTime - Time.time);
}

public float GetDamageMultiplier()
{
    return isDamageBuffActive ? damageMultiplier : 1f;
}


IEnumerator SignatureCoroutine()
{
    isDamageBuffActive = true;
    yield return new WaitForSeconds(signatureDuration);
    isDamageBuffActive = false;
}

[Header("Signature - Damage Buff")]
public float signatureCooldown = 30f;
public float signatureDuration = 5f;
public float damageMultiplier = 2f; // 100% more damage = double
private float nextSignatureReadyTime = 0f;
private bool isDamageBuffActive = false;

public float GetSignatureCooldownFraction()
{
    return Mathf.Clamp01((nextSignatureReadyTime - Time.time) / signatureCooldown);
}

public float GetSignatureSecondsRemaining()
{
    return Mathf.Max(0f, nextSignatureReadyTime - Time.time);
}
}