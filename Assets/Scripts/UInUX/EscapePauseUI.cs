using UnityEngine;

public class EscapePauseUI : MonoBehaviour
{
    public GameObject panel;
    private bool isPaused = false;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
{
    isPaused = true;
    panel.SetActive(true);
    Time.timeScale = 0f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    SetPlayerControlsEnabled(false);
    GetComponentInChildren<PauseTabController>()?.ShowItems();
}

    public void Resume()
    {
        isPaused = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetPlayerControlsEnabled(true);
    }

    void SetPlayerControlsEnabled(bool enabled)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerLook look = player.GetComponent<PlayerLook>();
        if (look != null) look.enabled = enabled;

        CameraPitch pitch = player.GetComponentInChildren<CameraPitch>();
        if (pitch != null) pitch.enabled = enabled;
    }
}