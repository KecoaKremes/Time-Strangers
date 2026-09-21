using UnityEngine;

public class CylinderAbilityUIAdapter : MonoBehaviour, ICharacterAbilityUI
{
    public PlayerAbilities abilities;
    public PlayerShooting shooting;

    void Awake() { CharacterAbilityRegistry.Current = this; }

    public float GetPrimaryFraction() => shooting.GetFireCooldownFraction();
    public float GetPrimarySeconds() => shooting.GetFireSecondsRemaining();
    public float GetSecondaryFraction() => abilities.GetAoECooldownFraction();
    public float GetSecondarySeconds() => abilities.GetAoESecondsRemaining();
    public float GetUtilityFraction() => abilities.GetDodgeCooldownFraction();
    public float GetUtilitySeconds() => abilities.GetDodgeSecondsRemaining();
    public float GetUltimateFraction() => abilities.GetUltimateCooldownFraction();
    public float GetUltimateSeconds() => abilities.GetUltimateSecondsRemaining();
    public float GetSignatureFraction() => abilities.GetSignatureCooldownFraction();
    public float GetSignatureSeconds() => abilities.GetSignatureSecondsRemaining();

    public Sprite primaryIcon, secondaryIcon, utilityIcon, ultimateIcon, signatureIcon;

public Sprite GetPrimaryIcon() => primaryIcon;
public Sprite GetSecondaryIcon() => secondaryIcon;
public Sprite GetUtilityIcon() => utilityIcon;
public Sprite GetUltimateIcon() => ultimateIcon;
public Sprite GetSignatureIcon() => signatureIcon;

public CharacterData characterData;

public AbilityInfo[] GetAbilityInfos()
{
    return new AbilityInfo[]
    {
        new AbilityInfo{ keyLabel = "Passive", slotName = "Passive", description = characterData.passiveDescription, icon = null },
        new AbilityInfo{ keyLabel = "M1", slotName = "Primary", description = characterData.primaryDescription, icon = primaryIcon },
        new AbilityInfo{ keyLabel = "M2", slotName = "Secondary", description = characterData.secondaryDescription, icon = secondaryIcon },
        new AbilityInfo{ keyLabel = "CTRL", slotName = "Utility", description = characterData.utilityDescription, icon = utilityIcon },
        new AbilityInfo{ keyLabel = "R", slotName = "Ultimate", description = characterData.ultimateDescription, icon = ultimateIcon },
        new AbilityInfo{ keyLabel = "V", slotName = "Signature", description = characterData.signatureDescription, icon = signatureIcon },
    };
}
}