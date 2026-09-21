using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public static DamageNumberSpawner Instance;
    public GameObject damageNumberPrefab;

    void Awake() { Instance = this; }

    public void Spawn(Vector3 worldPosition, int amount, bool isCrit)
    {
        GameObject obj = Instantiate(damageNumberPrefab, worldPosition + Vector3.up * 0.5f, Quaternion.identity);
        obj.GetComponent<DamageNumber>().Setup(amount, isCrit);
    }
}