using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparencyDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.4f;
    
    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    private Coroutine currentCoroutine;
    private Color originalColor;
    private int objectsInside = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();

        if (spriteRenderer)
            originalColor = spriteRenderer.color;
        else if (tilemap)
            originalColor = tilemap.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTrigger(other))
        {
            objectsInside++;

            if (objectsInside == 1)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(FadeCoroutine(transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidTrigger(other))
        {
            objectsInside = Mathf.Max(0, objectsInside - 1);

            if (objectsInside == 0)
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(FadeCoroutine(1f));
            }
        }
    }

    private bool IsValidTrigger(Collider2D other)
    {
        return other.GetComponent<PlayerController>() || other.GetComponent<EnemyAI>() || other.GetComponent<Pickup>();
    }

    private IEnumerator FadeCoroutine(float targetAlpha)
    {
        float elapsedTime = 0;
        float startAlpha = spriteRenderer ? spriteRenderer.color.a : tilemap.color.a;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeTime);

            if (spriteRenderer)
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            else if (tilemap)
                tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            yield return null;
        }

        if (spriteRenderer)
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
        else if (tilemap)
            tilemap.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

        currentCoroutine = null;
    }
}