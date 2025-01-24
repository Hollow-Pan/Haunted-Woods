using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour{

    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadowPrefab;
    [SerializeField] private GameObject grapeSplatterPrefab;

    private void Start() {
        GameObject grapeProjectileShadow = Instantiate(grapeProjectileShadowPrefab, transform.position + new Vector3(0f, -0.3f, 0f), Quaternion.identity);
        StartCoroutine(MoveProjectileShadowRoutine(grapeProjectileShadow, grapeProjectileShadow.transform.position, PlayerController.Instance.transform.position));
        StartCoroutine(ProjectileCurveRoutine(transform.position, PlayerController.Instance.transform.position));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition){
        float timePassed = 0f;
        while (timePassed < duration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }

        Instantiate(grapeSplatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveProjectileShadowRoutine(GameObject grapeProjectileShadow, Vector3 startPosition, Vector3 endPosition){
        float timePassed = 0f;
        while(timePassed < duration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT) / 2f;

            grapeProjectileShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            grapeProjectileShadow.transform.localScale = new Vector3(1 - heightT, 1 - heightT, 1);
            
            yield return null;
        }

        Destroy(grapeProjectileShadow);
    }

}