using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : Singleton<PointsManager>{

    private TMP_Text pointsAmountText;
    private int currentPoints = 0;

    const string COIN_AMOUNT_TEXT = "Point Amount Text";

    public void UpdateCurrentPoints(int incre){
        currentPoints += incre;

        if (pointsAmountText == null){
            pointsAmountText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        pointsAmountText.text = currentPoints.ToString("D3");
    }

}