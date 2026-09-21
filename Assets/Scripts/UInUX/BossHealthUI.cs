using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI : MonoBehaviour
{
    public static BossHealthUI Instance;

    public GameObject panel;
    public Slider slider;
    public TextMeshProUGUI bossNameText;

    private Health trackedHealth;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowBossHealth(Health bossHealth)
{
    trackedHealth = bossHealth;
    panel.SetActive(true);
    bossNameText.text = "Boss"; // or pull a real name field later
    slider.maxValue = bossHealth.maxHealth;
    slider.value = bossHealth.currentHealth;
    trackedHealth.onHealthChanged.AddListener(UpdateBar);
}

    void UpdateBar(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    public void HideBossHealth()
    {
        panel.SetActive(false);
        if (trackedHealth != null)
            trackedHealth.onHealthChanged.RemoveListener(UpdateBar);
    }
}