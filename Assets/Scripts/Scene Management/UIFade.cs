using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>{

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeCoroutine;

    public void FadeToBlack(){
        if (fadeCoroutine != null){
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = FadeCoroutine(1);
        StartCoroutine(fadeCoroutine);
    }

    public void FadeToClear(){
        if (fadeCoroutine != null){
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = FadeCoroutine(0);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator FadeCoroutine(float targetAlpha){
        while (!Mathf.Approximately(fadeImage.color.a, targetAlpha)){
            float alpha = Mathf.MoveTowards(fadeImage.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }

}
