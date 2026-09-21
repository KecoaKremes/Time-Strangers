using UnityEngine;

public class WorldItemPickup : MonoBehaviour
{
    public float spinSpeed = 120f;
    private ItemData assignedItem;
    private bool collected = false;
    private Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false; // disabled until the launch arc finishes
    }

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    public void AssignItem(ItemData item)
    {
        assignedItem = item;
    }

    public void EnablePickup()
    {
        col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
{
    if (collected || !other.CompareTag("Player")) return;
    PlayerInventory.Instance.AddItem(assignedItem);
    collected = true;
    Destroy(gameObject);
}
}