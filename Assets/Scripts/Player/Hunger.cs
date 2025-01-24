using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>{

    public int CurrentStamina {get; private set;}

    [SerializeField] private Sprite fullStaminaImage;
    [SerializeField] private Sprite emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefill = 2;

    private Transform staminaContainer;
    private int initialStamina = 5;
    private int maxStamina;

    const string STAMINA_CONTAINER = "Stamina Container";

    protected override void Awake() {
        base.Awake();

        maxStamina = initialStamina;
        CurrentStamina = initialStamina;
    }
    
    private void Start() {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER).transform;
    }

    public void UseStamina(){
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(RefillStaminaOverTimeRoutine());
    }

    public void RefillStamina(){
        if (CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead){
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    public void RefillStaminaOnDeath(){
        CurrentStamina = initialStamina;
        UpdateStaminaImages();
    }

    private IEnumerator RefillStaminaOverTimeRoutine(){
        while (true){
            yield return new WaitForSeconds(timeBetweenStaminaRefill);
            RefillStamina();
        }
    }

    private void UpdateStaminaImages(){
        for (int i = 0; i < maxStamina; i++){
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();
            if (i <= CurrentStamina - 1){
                image.sprite = fullStaminaImage;
            }
            else{
                image.sprite = emptyStaminaImage;
            }
        }
    }

}
