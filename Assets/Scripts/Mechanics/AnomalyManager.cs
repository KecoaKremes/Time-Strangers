using UnityEngine;
using UnityEngine.SceneManagement;

public enum RunState { Looting, BossFight, StageComplete }

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance;

    [Header("Timer")]
    public float stageDuration = 600f; // 10 minutes
    private float timeRemaining;
    public RunState currentState = RunState.Looting;

    [Header("References")]
    public EnemySpawner enemySpawner;
    public GameObject bossPrefab;
    public Transform anomalyPortalPoint;
    public GameObject portalIdleVisual;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = stageDuration;
    }

    void Update()
    {
        if (currentState == RunState.Looting)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                TriggerBossFight();
            }
        }
    }

    public float GetTimeRemaining() => timeRemaining;

    // Called by Chronos-rarity items later
    public void ReduceTime(float amount)
    {
        timeRemaining = Mathf.Max(0f, timeRemaining - amount);
    }

    public void OnPortalEntered()
    {
        if (currentState == RunState.Looting)
        {
            TriggerBossFight();
        }
        else if (currentState == RunState.StageComplete)
        {
            GoToNextStagePlaceholder();
        }
    }

    void TriggerBossFight()
    {
        currentState = RunState.BossFight;
        enemySpawner.enabled = false;
        portalIdleVisual.SetActive(false);

        GameObject boss = Instantiate(bossPrefab, anomalyPortalPoint.position, Quaternion.identity);
        Health bossHealth = boss.GetComponent<Health>();
        bossHealth.onDeath.AddListener(OnBossDefeated);
        

        if (BossHealthUI.Instance != null)
            BossHealthUI.Instance.ShowBossHealth(bossHealth);
    }

    public GameObject bossDropPrefab;
    void OnBossDefeated()
    {
        currentState = RunState.StageComplete;
        portalIdleVisual.SetActive(true);

        if (BossHealthUI.Instance != null)
            BossHealthUI.Instance.HideBossHealth();

        Instantiate(bossDropPrefab, anomalyPortalPoint.position + Vector3.up, Quaternion.identity);
    }

    void GoToNextStagePlaceholder()
    {
        Debug.Log("Stage complete — reloading as a placeholder for real stage progression.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddTime(float amount)
{
    timeRemaining += amount;
}
}