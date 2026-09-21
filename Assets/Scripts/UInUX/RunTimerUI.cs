using UnityEngine;
using TMPro;

public class RunTimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        float t = AnomalyManager.Instance.GetTimeRemaining();
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.color = t <= 60f ? Color.red : Color.white;
    }
}