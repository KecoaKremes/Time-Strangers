using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AoEProjectile : MonoBehaviour
{
    public int damage = 30;
    public float explosionRadius = 30f;
    public float lifetime = 5f;
    public string targetTag = "Enemy";

    private Rigidbody rb;
    private bool hasExploded = false;

void Awake()
{
    rb = GetComponent<Rigidbody>();
}

void Start()
{
    Destroy(gameObject, lifetime);
}
    

    public void Launch(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                Health targetHealth = hit.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
    
}