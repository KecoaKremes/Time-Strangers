using UnityEngine;

public class CameraPitch : MonoBehaviour
{
    public float mouseSensitivity = 3f;
    public float minPitch = -40f; // how far down you can look
    public float maxPitch = 70f;  // how far up you can look

    private float currentPitch = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentPitch -= mouseY; // subtract: moving mouse up should look up, not down
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        transform.localEulerAngles = new Vector3(currentPitch, 0f, 0f);
    }
}