using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectionManager : MonoBehaviour
{
    public static GameSelectionManager Instance;
    public CharacterData selectedCharacter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // prevent duplicates if this scene reloads
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        selectedCharacter = character;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}