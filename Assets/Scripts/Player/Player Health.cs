using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>{

    public bool IsDead {get; private set;}

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrust = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Leaderboard leaderboard;

    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float newOrthoSize = 2f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER = "Health Slider";
    const string RESPAWN_SCENE = "TownScene";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake() {
        base.Awake();

        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start() {
        IsDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCamera == null)
        {
            Debug.LogWarning("No CinemachineVirtualCamera found in the scene!");
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();

        if(enemyAI){
            TakeDamage(1, other.gameObject.transform);
        }
    }

    public void HealPlayer(){
        if (currentHealth < maxHealth){
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform){
        if (!canTakeDamage) {return;}

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockbackThrust);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckPlayerDeath();
    }

    private void CheckPlayerDeath(){
        if (currentHealth <= 0 && !IsDead){
            IsDead = true;
            currentHealth = 0;
            Destroy(ActiveWeapon.Instance.gameObject);
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            leaderboard.AddScoreToLeaderboard(PointsManager.Instance.DeathFlag());
            StartCoroutine(LerpOrthoSize());
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator LerpOrthoSize(){
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < 0.6f){
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 0.6f);
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, newOrthoSize, t);
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = newOrthoSize;
    }

    private IEnumerator DeathLoadSceneRoutine(){
        yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
        Stamina.Instance.RefillStaminaOnDeath();
        DeathScreenUI.Instance.Show();
        DeathScreenUI.Instance.UpdateVisual();
    }

    private IEnumerator DamageRecoveryRoutine(){
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider(){
        if (healthSlider == null){
            healthSlider = GameObject.Find(HEALTH_SLIDER).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

}
