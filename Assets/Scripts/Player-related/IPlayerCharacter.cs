using UnityEngine;

public interface IPlayerCharacter
{
    Health GetHealth();
    PlayerMovement GetMovement();
    Transform GetCameraPivot();
}