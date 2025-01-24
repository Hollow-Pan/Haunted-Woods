using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour{

    private enum PickupType{
        GoldCoin,
        HealthGlobe,
        StaminaGlobe,
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private float initialMoveSpeed = 3f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 0.5f;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update() {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickupDistance){
            moveDir = (playerPos - transform.position).normalized;
            initialMoveSpeed += acceleration;
        }
        else{
            moveDir = Vector3.zero;
            initialMoveSpeed = 0f;
        }
    }

    private void FixedUpdate() {
        rb.velocity = moveDir * initialMoveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController){
            DetectPickupType(); 
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine(){
        Vector2 startPosition = transform.position;
        float endPositionRandomX  = transform.position.x + Random.Range(-2f, 2f);
        float endPositionRandomY = transform.position.y + Random.Range(-1f, 1f);
        Vector2 endPosition = new Vector2(endPositionRandomX, endPositionRandomY);

        float timePassed = 0f;
        while (timePassed < popDuration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }

    private void DetectPickupType(){
        switch (pickupType){
            case PickupType.GoldCoin:
            EconomyManager.Instance.UpdateCurrentGold();
            break;

            case PickupType.HealthGlobe:
            PlayerHealth.Instance.HealPlayer();
            break;

            case PickupType.StaminaGlobe:
            Stamina.Instance.RefillStamina();
            break;

            default:
            break;
        }
    }

}
