using UnityEngine;

public class RavenCharacterRoot : MonoBehaviour, IPlayerCharacter
{
    public Health health;
    public PlayerMovement movement;
    public Transform cameraPivot;

    public Health GetHealth() => health;
    public PlayerMovement GetMovement() => movement;
    public Transform GetCameraPivot() => cameraPivot;
}