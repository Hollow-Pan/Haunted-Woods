using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>{

    [SerializeField] private List<SceneMusic> sceneMusicList = new List<SceneMusic>();

    private AudioSource audioSource;
    private float volume = 0.5f;

    protected override void Awake() {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName){
        SceneMusic sceneMusic = sceneMusicList.Find(music => music.sceneName == sceneName);

        if (sceneMusic != null && sceneMusic.musicClip != null) {
            if (audioSource.clip != sceneMusic.musicClip) {
                audioSource.clip = sceneMusic.musicClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        } else {
            Debug.LogWarning($"No music assigned for scene: {sceneName}");
        }
    }

    public void ChangeVolume(){
        volume += 0.1f;
        if (volume > 1f){
            volume = 0f;
        }
        audioSource.volume = volume;
    }

    public float GetVolume(){
        return volume;
    }

}

[System.Serializable]
public class SceneMusic {
    public string sceneName;
    public AudioClip musicClip;
}