using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -6f);
    public float followSpeed = 8f;          // how fast position catches up
    public float rotationFollowSpeed = 2f;  // how fast the camera's angle catches up (lower = lazier, more stable)

    private float currentYaw;

    void LateUpdate()
    {
        if (target == null) return;

        float targetYaw = target.eulerAngles.y;
        currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, rotationFollowSpeed * Time.deltaTime);

        Quaternion smoothedRotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 rotatedOffset = smoothedRotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}