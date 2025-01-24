using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu>{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        resumeButton.onClick.AddListener(() => {
            PauseMenuUI.Instance.TogglePauseMenu();
        });
        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenuScene");
        });
    }

    private void Start() {
        PauseMenuUI.Instance.OnGamePaused += PauseMenuUI_OnGamePaused;
        PauseMenuUI.Instance.OnGameUnpaused += PauseMenuUI_OnGameUnpaused;
        
        Hide();
    }

    private void PauseMenuUI_OnGamePaused(object sender, System.EventArgs e){
        Show();
    }

    private void PauseMenuUI_OnGameUnpaused(object sender, System.EventArgs e){
        Hide();
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

}
