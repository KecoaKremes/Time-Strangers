using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // try to re-fetch, in case it changed
            if (mainCamera == null) return; // still none available, skip this frame
        }

        transform.forward = mainCamera.transform.forward;
    }
}