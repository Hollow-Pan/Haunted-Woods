using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : Singleton<SettingsUI>{

    protected override bool ShouldDestroyInScene(string sceneName)
    {
        return sceneName == "MainMenuScene";
    }

   // [SerializeField] private Button SFXButton;
    [SerializeField] private Button musicButton;
   // [SerializeField] private TextMeshProUGUI SFXText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button backButton;

    protected override void Awake() {
        base.Awake();
        /*SFXButton.onClick.AddListener(() => {

        });*/
    }

    private void OnEnable() {
        if (musicButton != null){
            musicButton.onClick.AddListener(() => {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });
        }
        if (backButton != null){
            backButton.onClick.AddListener(() => {
                Hide();
            });
        }
    }

    private void OnDisable() {
        if (musicButton != null){
            musicButton.onClick.RemoveListener(() => {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });
        }
        if (backButton != null){
            backButton.onClick.AddListener(() => {
                Hide();
            });
        }
    }

    private void Start() {
        PauseMenuUI.Instance.OnGameUnpaused += PauseMenuUI_OnGameUnpaused;

        UpdateVisual();

        Hide();
    }

    private void PauseMenuUI_OnGameUnpaused(object sender, System.EventArgs e){
        Hide();
    }

    private void UpdateVisual(){
        musicText.text = "Music Volume: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    protected override void OnDestroy() {
        base.OnDestroy(); // Unsubscribe from scene events in Singleton<T>
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

}
