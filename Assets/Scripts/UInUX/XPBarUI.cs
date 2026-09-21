using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI levelText;

    void Start()
    {
        PlayerProgression.Instance.onXPChanged.AddListener(UpdateBar);
    }

    void UpdateBar(int level, int currentXP, int xpToNext)
    {
        levelText.text = "Lv. " + level;
        slider.maxValue = xpToNext;
        slider.value = currentXP;
    }
}