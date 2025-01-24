using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>{

    public bool IsDead {get; private set;}

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrust = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

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
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.RefillStaminaOnDeath();
        SceneManager.LoadScene(RESPAWN_SCENE);
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
