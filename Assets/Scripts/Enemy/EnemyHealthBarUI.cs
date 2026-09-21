using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    private Health enemyHealth;
    private Slider slider;

    void Start()
{
    enemyHealth = GetComponentInParent<Health>();
    slider = GetComponentInChildren<Slider>();
    enemyHealth.onHealthChanged.AddListener(UpdateBar);
}

    void UpdateBar(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    void OnDestroy()
    {
        if (enemyHealth != null)
            enemyHealth.onHealthChanged.RemoveListener(UpdateBar);
    }
}