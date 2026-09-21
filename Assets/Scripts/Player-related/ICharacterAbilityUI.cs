using UnityEngine;

public interface ICharacterAbilityUI
{
    float GetPrimaryFraction(); float GetPrimarySeconds(); Sprite GetPrimaryIcon();
    float GetSecondaryFraction(); float GetSecondarySeconds(); Sprite GetSecondaryIcon();
    float GetUtilityFraction(); float GetUtilitySeconds(); Sprite GetUtilityIcon();
    float GetUltimateFraction(); float GetUltimateSeconds(); Sprite GetUltimateIcon();
    float GetSignatureFraction(); float GetSignatureSeconds(); Sprite GetSignatureIcon();

    AbilityInfo[] GetAbilityInfos();
}