using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour{

    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.GetComponent<PlayerController>()){
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            Debug.Log(sceneTransitionName);

            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneCoroutine());
        }
    }

    private IEnumerator LoadSceneCoroutine(){  
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);      
    }

}