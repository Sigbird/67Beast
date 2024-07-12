using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int CapturedEnemies;
    public static int PlayerCapacity = 4;
    public static UiManager uiManager;
    public static int PlayerCash = 0;

    void Start()
    {
        uiManager = FindAnyObjectByType<UiManager>();

        uiManager.GameScoreText.text = "Capacity:\n" + CapturedEnemies + "/" + PlayerCapacity;
    }
}
