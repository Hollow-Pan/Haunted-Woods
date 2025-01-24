using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeSplatter : MonoBehaviour{

    [SerializeField] private float disableColliderTime = 0.5f;

    private SpriteFade spriteFade;

    private void Awake() {
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start() {
        StartCoroutine(spriteFade.SlowFadeCoroutine());

        Invoke("DisableCollider", disableColliderTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth){
            playerHealth.TakeDamage(1, this.transform);
        }
    }

    private void DisableCollider(){
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

}