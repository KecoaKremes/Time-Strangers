using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 3f;

    void Start()
    {
        // Lock cursor to center of screen and hide it — standard for mouse-look games
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }
}