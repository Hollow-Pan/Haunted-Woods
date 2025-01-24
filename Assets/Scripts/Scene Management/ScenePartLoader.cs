using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod{
    Distance,
    Trigger,
}

public class ScenePartLoader : MonoBehaviour{

    [SerializeField] private CheckMethod checkMethod;
    [SerializeField] private float loadRange;

    private bool isLoaded;
    private bool shouldLoad;

    private void Start() {
        if (SceneManager.sceneCount > 0){
            for (int i = 0; i < SceneManager.sceneCount; i++){
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name){
                    isLoaded = true;
                }
            }
        }
    }

    private void Update() {
        if (checkMethod == CheckMethod.Distance){
            DistanceCheck();
        }
        else{
            TriggerCheck();
        }
    }

    private void DistanceCheck(){
        if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < loadRange){
            LoadScene();
        }
        else{
            UnloadScene();
        }
    }

    private void TriggerCheck(){

    }

    private void LoadScene(){
        if (!isLoaded){
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    private void UnloadScene(){
        if (isLoaded){
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

}
