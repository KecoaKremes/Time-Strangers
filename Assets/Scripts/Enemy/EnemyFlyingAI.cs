using UnityEngine;

public class EnemyFlyingAI : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public float flightHeight = 4f;
    public float preferredDistance = 8f;
    public float moveSpeed = 4f;
    public float fireRate = 10f;
    public int damage = 8;

    private float nextFireTime;

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }
    }

    public LayerMask obstacleMask; // set to "Environment" layer
public float avoidanceDistance = 2f;

void Update()
{
    if (target == null) return;

    Vector3 desiredPosition = target.position;
    desiredPosition.y = flightHeight;

    float distance = Vector3.Distance(transform.position, target.position);

    Vector3 moveTarget;
    if (distance > preferredDistance + 1f)
    {
        moveTarget = desiredPosition;
    }
    else if (distance < preferredDistance - 1f)
    {
        moveTarget = transform.position + (transform.position - desiredPosition).normalized;
    }
    else
    {
        moveTarget = transform.position; // hovering, no horizontal movement needed
    }

    Vector3 moveDirection = (moveTarget - transform.position).normalized;

    // Check if something's directly in the way before committing to that direction
    if (Physics.SphereCast(transform.position, 0.5f, moveDirection, out RaycastHit hit, avoidanceDistance, obstacleMask))
    {
        // Something's blocking — steer up and away from it instead
        Vector3 avoidDirection = (Vector3.up + hit.normal).normalized;
        transform.position += avoidDirection * moveSpeed * Time.deltaTime;
    }
    else
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    transform.LookAt(target.position);

    if (Time.time >= nextFireTime)
    {
        Shoot();
        nextFireTime = Time.time + fireRate;
    }
}
    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.targetTag = "Player";
            projScript.isPlayerOwned = false; // this projectile only hurts the player
            projScript.damage = damage;
        }
    }

    
}