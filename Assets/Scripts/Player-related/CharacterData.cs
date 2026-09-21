using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName = "Unnamed";
    public bool isUnlocked = false;
    public Sprite portrait;
    public GameObject characterPrefab;

    [Header("Abilities")]
    [TextArea] public string passiveDescription;
    [TextArea] public string primaryDescription;
    [TextArea] public string secondaryDescription;
    [TextArea] public string utilityDescription;
    [TextArea] public string ultimateDescription;
    [TextArea] public string signatureDescription;
}