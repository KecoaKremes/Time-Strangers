using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnomalyManager.Instance.OnPortalEntered();
        }
    }
}