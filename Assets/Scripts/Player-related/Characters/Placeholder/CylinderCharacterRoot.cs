using UnityEngine;

public class CylinderCharacterRoot : MonoBehaviour, IPlayerCharacter
{
    public Health health;
    public PlayerMovement movement;
    public Transform cameraPivot;

    public Health GetHealth() => health;
    public PlayerMovement GetMovement() => movement;
    public Transform GetCameraPivot() => cameraPivot;
}