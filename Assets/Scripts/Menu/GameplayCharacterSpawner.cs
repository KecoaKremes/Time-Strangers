using UnityEngine;

public class GameplayCharacterSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public CharacterData fallbackCharacter; // for testing Gameplay scene directly

    void Awake()
    {
        CharacterData toSpawn = (GameSelectionManager.Instance != null && GameSelectionManager.Instance.selectedCharacter != null)
            ? GameSelectionManager.Instance.selectedCharacter
            : fallbackCharacter;

        if (toSpawn == null || toSpawn.characterPrefab == null)
        {
            Debug.LogError("No character to spawn — check GameSelectionManager or assign a Fallback Character.");
            return;
        }

        GameObject instance = Instantiate(toSpawn.characterPrefab, spawnPoint.position, spawnPoint.rotation);
        WireDependencies(instance);
    }

    void WireDependencies(GameObject player)
    {
        IPlayerCharacter character = player.GetComponent<IPlayerCharacter>();
        if (character == null)
        {
            Debug.LogError("Spawned character is missing an IPlayerCharacter root component.");
            return;
        }

        Health health = character.GetHealth();
        PlayerMovement movement = character.GetMovement();

        if (PlayerProgression.Instance != null) PlayerProgression.Instance.playerHealth = health;
        if (ItemEffectSystem.Instance != null)
        {
            ItemEffectSystem.Instance.playerHealth = health;
            ItemEffectSystem.Instance.playerMovement = movement;
        }

        CameraCollision camCollision = player.GetComponentInChildren<CameraCollision>();
        // CameraCollision already lives inside the prefab and self-references its own child camera, so nothing extra needed here.
    }
}