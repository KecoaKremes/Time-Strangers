using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform cameraTransform;   // drag Main Camera in
    public Vector3 desiredLocalOffset = new Vector3(0f, 0.5f, -6f);
    public float collisionRadius = 0.3f; // thickness of the camera "feeler"
    public LayerMask collisionMask;      // what counts as an obstruction
    public float smoothSpeed = 15f;

    void LateUpdate()
    {
        Vector3 desiredWorldPos = transform.TransformPoint(desiredLocalOffset);
        Vector3 direction = desiredWorldPos - transform.position;
        float desiredDistance = direction.magnitude;
        direction.Normalize();

        float finalDistance = desiredDistance;

        if (Physics.SphereCast(transform.position, collisionRadius, direction, out RaycastHit hit, desiredDistance, collisionMask))
        {
            finalDistance = hit.distance;
        }

        Vector3 targetPosition = transform.position + direction * finalDistance;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}