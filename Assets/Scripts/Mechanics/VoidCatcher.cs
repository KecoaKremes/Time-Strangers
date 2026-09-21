using UnityEngine;

public class VoidCatcher : MonoBehaviour
{
    public float killHeight = -100f; // world Y below which anything gets caught
    public Vector3 respawnPoint = new Vector3(0f, 2f, 0f);

    void Update()
    {
        if (transform.position.y < killHeight)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; // must disable before teleporting a CharacterController

            transform.position = respawnPoint;

            Health health = GetComponent<Health>();
            if (health != null) health.TakeDamage(20); // small penalty for going out of bounds

            if (cc != null) cc.enabled = true;
        }
    }
}