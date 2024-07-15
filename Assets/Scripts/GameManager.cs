using UnityEngine;

/// <summary>
/// Keeps the reference and access to the main variables of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    public static int CapturedEnemies;
    public static int PlayerCapacity = 4;
    public static UiManager UiManager;
    public static int PlayerCash = 0;
    public static CharacterController characterController;

    private void Start()
    {
        characterController = FindAnyObjectByType<CharacterController>();

        //Get the UI manager reference and reset it.
        UiManager = FindAnyObjectByType<UiManager>();
        UiManager.GameScoreText.text = "Capacity:\n" + CapturedEnemies + "/" + PlayerCapacity;
    }
}
