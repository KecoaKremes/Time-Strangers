using UnityEngine;
using System.Collections;

public class BleedEffect : MonoBehaviour
{
    public void Apply(Health target, int totalDamage, float duration, int ticks)
    {
        StartCoroutine(BleedTick(target, totalDamage, duration, ticks));
    }

    IEnumerator BleedTick(Health target, int totalDamage, float duration, int ticks)
    {
        int perTick = Mathf.Max(1, totalDamage / ticks);
        float interval = duration / ticks;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(interval);
            if (target != null) target.TakeDamage(perTick);
        }
        Destroy(this);
    }
}