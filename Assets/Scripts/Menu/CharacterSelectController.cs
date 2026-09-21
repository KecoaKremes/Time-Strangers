using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterSelectController : MonoBehaviour
{
    public List<CharacterData> allCharacters;
    public Transform gridParent;
    public GameObject slotPrefab;

    [Header("Info Panel")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI passiveText;
    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondaryText;
    public TextMeshProUGUI utilityText;
    public TextMeshProUGUI ultimateText;
    public TextMeshProUGUI signatureText;
    public Button confirmButton;

    private CharacterData highlightedCharacter;

    void Start()
    {
        foreach (CharacterData character in allCharacters)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);

            Image lockOverlay = slot.transform.Find("LockOverlay").GetComponent<Image>();
            lockOverlay.enabled = !character.isUnlocked;

            Image portraitImage = slot.GetComponent<Image>();
            if (character.portrait != null) portraitImage.sprite = character.portrait;

            Button slotButton = slot.GetComponent<Button>();
            slotButton.interactable = character.isUnlocked;

            // Capture the character in a local variable for the closure below
            CharacterData capturedCharacter = character;
            slotButton.onClick.AddListener(() => HighlightCharacter(capturedCharacter));
        }

        confirmButton.interactable = false;
    }

    void HighlightCharacter(CharacterData character)
    {
        highlightedCharacter = character;

        nameText.text = character.characterName;
        passiveText.text = "Passive: " + character.passiveDescription;
        primaryText.text = "Primary: " + character.primaryDescription;
        secondaryText.text = "Secondary: " + character.secondaryDescription;
        utilityText.text = "Utility: " + character.utilityDescription;
        ultimateText.text = "Ultimate: " + character.ultimateDescription;
        signatureText.text = "Signature: " + character.signatureDescription;

        confirmButton.interactable = true;
    }

    public void OnConfirmPressed()
    {
        if (highlightedCharacter == null) return;

        GameSelectionManager.Instance.SelectCharacter(highlightedCharacter);
        GameSelectionManager.Instance.LoadScene("Gameplay");
    }
}