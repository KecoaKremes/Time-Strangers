using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    public Health playerHealth;
    public Slider slider;

   void Start()
{
    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

    playerHealth.onHealthChanged.AddListener(UpdateBar);
}

    void UpdateBar(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    void OnDestroy()
    {
        // Good practice: unsubscribe so nothing tries to call into a destroyed object
        playerHealth.onHealthChanged.RemoveListener(UpdateBar);
    }
}