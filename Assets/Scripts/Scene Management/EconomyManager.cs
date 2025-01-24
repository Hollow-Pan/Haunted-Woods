using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>{

    private TMP_Text goldAmountText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    public void UpdateCurrentGold(){
        currentGold += 1;

        if (goldAmountText == null){
            goldAmountText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldAmountText.text = currentGold.ToString("D3");
    }

}
