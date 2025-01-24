using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : Singleton<PauseMenuUI>{

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private PlayerControls playerControls;
    private bool isPaused = false;

    protected override void Awake(){
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        playerControls.UI.PauseMenu.performed += _ => TogglePauseMenu();
    }

    public void TogglePauseMenu(){
        isPaused = !isPaused;
        if (isPaused){
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else{
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

}