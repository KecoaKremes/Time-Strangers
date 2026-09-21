using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldownUI : MonoBehaviour
{
    public Image m1Fill, m2Fill, dodgeFill, ultimateFill, signatureFill;
    public Image m1Icon, m2Icon, dodgeIcon, ultimateIcon, signatureIcon;
    public TextMeshProUGUI m1Text, m2Text, dodgeText, ultimateText, signatureText;

    private ICharacterAbilityUI lastCharacter;

    void Update()
    {
        ICharacterAbilityUI c = CharacterAbilityRegistry.Current;
        if (c == null) return;

        if (!ReferenceEquals(c, lastCharacter))
        {
            ApplyIcons(c);
            lastCharacter = c;
        }

        UpdateSlot(m1Fill, m1Text, c.GetPrimaryFraction(), c.GetPrimarySeconds());
        UpdateSlot(m2Fill, m2Text, c.GetSecondaryFraction(), c.GetSecondarySeconds());
        UpdateSlot(dodgeFill, dodgeText, c.GetUtilityFraction(), c.GetUtilitySeconds());
        UpdateSlot(ultimateFill, ultimateText, c.GetUltimateFraction(), c.GetUltimateSeconds());
        UpdateSlot(signatureFill, signatureText, c.GetSignatureFraction(), c.GetSignatureSeconds());
    }

    void ApplyIcons(ICharacterAbilityUI c)
    {
        SetIcon(m1Icon, c.GetPrimaryIcon());
        SetIcon(m2Icon, c.GetSecondaryIcon());
        SetIcon(dodgeIcon, c.GetUtilityIcon());
        SetIcon(ultimateIcon, c.GetUltimateIcon());
        SetIcon(signatureIcon, c.GetSignatureIcon());
    }

    void SetIcon(Image target, Sprite sprite)
    {
        if (target == null) return;
        if (sprite != null) { target.sprite = sprite; target.color = Color.white; target.enabled = true; }
        else target.enabled = false;
    }

    void UpdateSlot(Image fill, TextMeshProUGUI text, float fraction, float secondsRemaining)
    {
        fill.fillAmount = fraction;
        if (secondsRemaining > 0.05f) { text.text = secondsRemaining.ToString("F1"); text.enabled = true; }
        else text.enabled = false;
    }
}