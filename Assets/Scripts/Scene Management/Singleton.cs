using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T: Singleton<T> {

    private static T instance;
    public static T Instance {get {return instance;}}

    protected virtual void Awake(){
        if (instance != null && this.gameObject != null){
            Destroy(this.gameObject);
        }
        else{
            instance = (T)this;
        }

        if (!gameObject.transform.parent){
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDestroy() {
        if (instance == this) {
            instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (ShouldDestroyInScene(scene.name)) {
            Destroy(gameObject);
        }
    }

    protected virtual bool ShouldDestroyInScene(string sceneName) {
        return false;
    }

}
