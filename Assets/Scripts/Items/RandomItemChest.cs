using UnityEngine;
using TMPro;

public class RandomItemChest : MonoBehaviour
{
    public int cost = 25;
    public MeshRenderer visualRenderer;
    public TextMeshPro costLabel;
    public TextMeshPro interactPrompt;
    public Color closedColor = Color.gray;

    public float revealRadius = 8f;
    public float interactRadius = 3f;

    private bool opened = false;
    private bool playerInRange = false;
    private Transform playerTransform;

    void Start()
    {
        if (visualRenderer != null) visualRenderer.material.color = closedColor;
        if (costLabel != null) costLabel.text = cost + " Coins";
        costLabel.gameObject.SetActive(false);
        interactPrompt.gameObject.SetActive(false);
        interactPrompt.text = "[E] Open";
    }

    void Update()
    {
        if (opened || playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        bool shouldShowCost = distance <= revealRadius;
        if (costLabel.gameObject.activeSelf != shouldShowCost)
            costLabel.gameObject.SetActive(shouldShowCost);

        bool nowInRange = distance <= interactRadius;
        if (interactPrompt.gameObject.activeSelf != nowInRange)
            interactPrompt.gameObject.SetActive(nowInRange);

        playerInRange = nowInRange;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpen();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerTransform = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            costLabel.gameObject.SetActive(false);
            interactPrompt.gameObject.SetActive(false);
        }
    }

    void TryOpen()
{
    if (!PlayerProgression.Instance.SpendCoins(cost)) return;

    ItemRarity rolledRarity = RollRarity();
    ItemData rolledItem = ItemDatabase.Instance.GetRandomItemOfRarity(rolledRarity);

    if (rolledItem == null)
    {
        PlayerProgression.Instance.AddCoins(cost);
        return;
    }

    opened = true;
    PlayerInventory.Instance.AddItem(rolledItem);

    if (rolledItem.worldModelPrefab != null) visualRenderer.enabled = false;
    else visualRenderer.material.color = rolledItem.placeholderColor;
    costLabel.text = rolledItem.itemName;
    interactPrompt.gameObject.SetActive(false);
}

    ItemRarity RollRarity()
    {
        float roll = Random.value;
        if (roll < 0.70f) return ItemRarity.Common;
        if (roll < 0.95f) return ItemRarity.Uncommon;
        return ItemRarity.Legendary;
    }
}