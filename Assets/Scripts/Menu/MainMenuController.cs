using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayPressed()
    {
        GameSelectionManager.Instance.LoadScene("CharacterSelect");
    }

    public void OnExitPressed()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}