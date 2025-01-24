using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour {

    [SerializeField] private float laserGrowTime = .22f;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private bool isGrowing = true;

    private void Awake() { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start() {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.GetComponent<Indestructible>() && !other.isTrigger){
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange){
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthCoroutine());
    }

    private IEnumerator IncreaseLaserLengthCoroutine(){
        float timePassed = 0f;
        while(spriteRenderer.size.x < laserRange && isGrowing){
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            capsuleCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider.size.y);
            capsuleCollider.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT) / 2, capsuleCollider.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeCoroutine());
    }

    private void LaserFaceMouse(){
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }
    
}
