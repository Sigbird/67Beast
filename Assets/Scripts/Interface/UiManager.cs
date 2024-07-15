using TMPro;
using UnityEngine;

/// <summary>
/// Keep the reference of main elements of the UI for other Scripts to access and implements basic update methods on the UI elements
/// </summary>
public class UiManager : MonoBehaviour
{   
    [Tooltip("Reference to the Game Score Text")]
    public TMP_Text GameScoreText;
    [Tooltip("Reference to the player cash Text")]
    public TMP_Text PlayerCashText;
    [Tooltip("Reference to the Deliver Button on the UI")]
    public GameObject DeliverButton;

    /// <summary>
    /// Called from the Button on the UI to Check if the player have enouth cash for the upgrade and upgrade in case it has.
    /// </summary>
    public void PurchaseUpgrade()
    {
        if (GameManager.PlayerCash >= 50)
        {
            //Upda the Game Manager Variables
            GameManager.PlayerCash -= 50;
            GameManager.PlayerCapacity += 6;
            GameManager.characterController.ChangePlayerColor(Color.red);

            //Update the elements on the UI
            UpdateCash();
            UpdateCapacity();
        }
    }

    /// <summary>
    /// Update the player Cash on the UI
    /// </summary>
    public void UpdateCash()
    {
        PlayerCashText.text = "Cash:\n" + GameManager.PlayerCash + "$";
    }

    /// <summary>
    /// Update the player capacity to carry enemies on the UI.
    /// </summary>
    public void UpdateCapacity()
    {
        GameScoreText.text = "Capacity:\n" + GameManager.CapturedEnemies + "/" + GameManager.PlayerCapacity;
    }
}
