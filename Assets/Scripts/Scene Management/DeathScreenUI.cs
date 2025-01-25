using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenUI : Singleton<DeathScreenUI>{

    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        mainMenuButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        Hide();
    }

    public void UpdateVisual(){
        currentScoreText.text = "SCORE: " + PointsManager.Instance.DeathFlag();
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

}