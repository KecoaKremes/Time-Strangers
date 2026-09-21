using UnityEngine;
using System.Collections;

public class ChestItemLauncher : MonoBehaviour
{
    public static ChestItemLauncher Instance;
    public GameObject itemPickupPrefab; // thin rotating rectangle placeholder

    void Awake() { Instance = this; }

    public void LaunchItem(Vector3 chestPosition, ItemData item, Transform chestTransform)
    {
        Vector3 landPosition = chestPosition + chestTransform.forward * 1.8f;
        GameObject pickup = Instantiate(itemPickupPrefab, chestPosition, Quaternion.identity);
        pickup.GetComponent<MeshRenderer>().material.color = item.placeholderColor;
        pickup.GetComponent<WorldItemPickup>().AssignItem(item);
        StartCoroutine(LaunchArc(pickup.transform, chestPosition, landPosition));
    }

    IEnumerator LaunchArc(Transform itemTransform, Vector3 startPos, Vector3 landPos)
    {
        float launchHeight = 6f;
        float upDuration = 0.5f;
        float fallDuration = 0.6f;
        Vector3 skyPoint = startPos + Vector3.up * launchHeight;

        float t = 0f;

        while (t < upDuration)
        {
            if (itemTransform == null)
                yield break;

            t += Time.deltaTime;
            itemTransform.position = Vector3.Lerp(startPos, skyPoint, t / upDuration);
            yield return null;
        }

        t = 0f;

        while (t < fallDuration)
        {
            if (itemTransform == null)
                yield break;

            t += Time.deltaTime;
            itemTransform.position = Vector3.Lerp(skyPoint, landPos, t / fallDuration);
            yield return null;
        }

        if (itemTransform == null)
            yield break;

        WorldItemPickup pickup = itemTransform.GetComponent<WorldItemPickup>();

        if (pickup != null)
            pickup.EnablePickup();
    }
}