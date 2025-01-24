using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue_Emie : MonoBehaviour{

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private AudioClip dialogueClip;
    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private float transparencyAmount = 1f;
    [SerializeField] private float volume = 1f;

    private Camera cam;
    private AudioSource audioSource;

    private void Awake() {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()){
            StartCoroutine(FadeCoroutine(dialogueText, fadeTime, dialogueText.color.a, transparencyAmount));
            if (!audioSource.isPlaying){
                audioSource.PlayOneShot(dialogueClip, volume);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>()){
            StartCoroutine(FadeCoroutine(dialogueText, fadeTime, dialogueText.color.a, 0));
        }
    }

    private IEnumerator FadeCoroutine(TMP_Text infoText, float fadeTime, float startValue, float targetTransparency){
        float elapsedTime = 0;
        while (elapsedTime < fadeTime){
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, newAlpha);
            yield return null;
        }
    }

}
