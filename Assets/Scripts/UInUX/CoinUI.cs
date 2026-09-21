using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        PlayerProgression.Instance.onCoinsChanged.AddListener(UpdateText);
        UpdateText(PlayerProgression.Instance.chronoCoins);
    }

    void UpdateText(int amount)
    {
        coinText.text = amount.ToString();
    }
}