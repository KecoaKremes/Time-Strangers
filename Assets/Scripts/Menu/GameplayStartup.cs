using UnityEngine;

public class GameplayStartup : MonoBehaviour
{
    void Start()
    {
        if (GameSelectionManager.Instance != null && GameSelectionManager.Instance.selectedCharacter != null)
        {
            Debug.Log("Playing as: " + GameSelectionManager.Instance.selectedCharacter.characterName);
        }
    }
}