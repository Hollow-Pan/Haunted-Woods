using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour{

    [SerializeField] private GameObject goldCoinPrefab, healthGlobePrefab, staminaGlobePrefab;

    public void DropItems(){
        int randomNum = Random.Range(1, 4);

        switch (randomNum){
            case 1:
            Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
            break;

            case 2:
            Instantiate(staminaGlobePrefab, transform.position, Quaternion.identity);
            break;

            case 3:
            int randomAmountOfGold = Random.Range(1, 4);
            for (int i = 0; i < randomAmountOfGold; i++){
                Instantiate(goldCoinPrefab, transform.position, Quaternion.identity);
            }
            break;

            default:
            break;
        }
    }

}
