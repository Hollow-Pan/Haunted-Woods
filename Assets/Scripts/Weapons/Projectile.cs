using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{

    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitVFXPrefab;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private bool isEnemyProjectile = false;

    private Rigidbody2D rb;
    private Vector3 startPosition;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        startPosition = this.transform.position;
    }

    private void Update(){
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange){
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
    }

    private void DetectFireDistance(){
        if (Vector3.Distance(this.transform.position, startPosition) > projectileRange){
            Instantiate(particleOnHitVFXPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void MoveProjectile(){
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other){
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();

        if (!other.isTrigger && (enemyHealth || indestructible || playerHealth)){
            if ((playerHealth && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile)){
                playerHealth?.TakeDamage(1, transform);
                Instantiate(particleOnHitVFXPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (!other.isTrigger && indestructible){
                Instantiate(particleOnHitVFXPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

    }

}
