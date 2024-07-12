using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public TMP_Text GameScoreText;
    public TMP_Text PlayerCashText;
    public GameObject DeliverButton;

    public void PurchaseUpgrade()
    {
        if (GameManager.PlayerCash > 50)
        {
            GameManager.PlayerCash -= 50;
            GameManager.PlayerCapacity += 6;
            UpdateCash();
            UpdateCapacity();
        }
    }

    public void UpdateCash(){
        PlayerCashText.text = "Cash:\n"+GameManager.PlayerCash+"$";
    }

    public void UpdateCapacity(){
        GameScoreText.text = "Capacity:\n"+GameManager.CapturedEnemies+"/"+GameManager.PlayerCapacity;
    }
}
