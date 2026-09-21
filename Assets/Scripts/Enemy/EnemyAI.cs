using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform target; // the player
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    private NavMeshAgent agent;
    private float nextAttackTime;

    void Start()
{
    agent = GetComponent<NavMeshAgent>();
    if (target == null)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
    }
}

    void Update()
    {
        if (isStunned) return;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            agent.SetDestination(target.position); // walk toward player
        }
        else
        {
            agent.SetDestination(transform.position); // stop moving to attack
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            Health playerHealth = target.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private bool isStunned = false;

    public void ApplyStun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

       IEnumerator StunRoutine(float duration)
{
    isStunned = true;

    if (agent.isActiveAndEnabled && agent.isOnNavMesh)
    {
        agent.isStopped = true;
    }

    yield return new WaitForSeconds(duration);

    if (agent.isActiveAndEnabled && agent.isOnNavMesh)
    {
        agent.isStopped = false;
    }

    isStunned = false;
}
}