using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class BossAI : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Slam")]
    public float slamRange = 4f;
    public int slamDamage = 25;
    public float slamRadius = 5f;
    public float slamTelegraph = 0.8f;

    [Header("Burst")]
    public float burstRange = 15f;
    public int burstProjectileCount = 5;
    public int burstDamage = 10;

    [Header("Charge")]
    public float chargeRange = 10f;
    public float chargeSpeed = 20f;
    public int chargeDamage = 20;

    [Header("General")]
    public float attackCooldown = 3f;

    private NavMeshAgent agent;
    private Health health;
    private Renderer rend;
    private Color baseColor;
    private float nextAttackTime;
    private bool isBusy = false;
    private bool inPhaseTwo = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;

        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (isStunned) return;
        if (target == null || isBusy) return;

        if (!inPhaseTwo && health.currentHealth <= health.maxHealth / 2)
        {
            inPhaseTwo = true;
            attackCooldown *= 0.6f;
            chargeSpeed *= 1.3f;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (Time.time >= nextAttackTime)
        {
            if (distance <= slamRange) StartCoroutine(SlamAttack());
            else if (distance <= chargeRange && distance > slamRange) StartCoroutine(ChargeAttack());
            else if (distance <= burstRange) StartCoroutine(BurstAttack());
            else agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    IEnumerator Telegraph(float duration, Color warnColor)
    {
        isBusy = true;
        agent.isStopped = true;
        rend.material.color = warnColor;
        yield return new WaitForSeconds(duration);
        rend.material.color = baseColor;
    }

    IEnumerator SlamAttack()
    {
        yield return Telegraph(slamTelegraph, Color.red);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= slamRadius)
        {
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth != null) targetHealth.TakeDamage(slamDamage);
        }

        agent.isStopped = false;
        isBusy = false;
        nextAttackTime = Time.time + attackCooldown;
    }

    IEnumerator BurstAttack()
    {
        yield return Telegraph(0.5f, Color.yellow);

        Vector3 baseDir = (target.position - firePoint.position).normalized;
        for (int i = 0; i < burstProjectileCount; i++)
        {
            float angle = (i - burstProjectileCount / 2f) * 8f;
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null) { p.targetTag = "Player"; p.damage = burstDamage; p.isPlayerOwned = false; }
        }

        agent.isStopped = false;
        isBusy = false;
        nextAttackTime = Time.time + attackCooldown;
    }

    IEnumerator ChargeAttack()
    {
        yield return Telegraph(0.6f, Color.magenta);

        agent.isStopped = false;
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 destination = transform.position + direction * (chargeRange + 3f);
        agent.speed = chargeSpeed;
        agent.SetDestination(destination);

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= 2f)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null) targetHealth.TakeDamage(chargeDamage);
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.speed = 3.5f; // reset to normal walk speed
        isBusy = false;
        nextAttackTime = Time.time + attackCooldown;
    }
    private bool isStunned = false;

    public void ApplyStun(float duration)
    {
    StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
    isStunned = true;
    agent.isStopped = true;
    yield return new WaitForSeconds(duration);
    agent.isStopped = false;
    isStunned = false;
    }
}