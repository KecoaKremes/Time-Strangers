using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Health playerHealth;
    public GameObject gameOverScreen;

    void Start()
{
    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

    playerHealth.onDeath.AddListener(HandleGameOver);
    gameOverScreen.SetActive(false);
}

    void HandleGameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // freezes all physics/Update-based movement globally

        // Cursor needs to be freed so the player can click a restart button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // must reset before reloading, or the new scene loads paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}