using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the punch input and trigger its efects on enemies stacking then and applying the efects on them.
/// </summary>
public class PunchHandler : MonoBehaviour
{
    //Constants
    private const string DELIVER_ZONE_TAG = "DeliverZone";
    private const string ENEMY_TAG = "Enemy";

    //References
    [Tooltip("The point on the player's back where enemies will be stacked")]
    [SerializeField] private Transform _stackPoint;
    [Tooltip("Offset for stacking enemies")]
    [SerializeField] private float _stackOffset = 1.0f;
    [Tooltip("Delay after punch")]
    [SerializeField] private float _delayAfterPunch = 3f;
    private Animator _animator;
    /// <summary>
    /// List of Enemies
    /// </summary>
    private List<WavingEnemy> _stackedEnemies = new();

    //Control Booleans    
    private bool _collisionFlag;
    private bool _insideDeliverArea;
    private bool _isPunching;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        UpdateStackedEnemies();
    }

    /// <summary>
    /// Continuously update the enemies positions to be atached to the player back or the latest enemy added on it.
    /// </summary>
    private void UpdateStackedEnemies()
    {
        for (int i = 0; i < _stackedEnemies.Count; i++)
        {
            if (i == 0)
            {
                _stackedEnemies[i].SetTarget(_stackPoint.position + new Vector3(0, _stackOffset, 0), _stackPoint.rotation, 1000);
            }
            else
            {
                _stackedEnemies[i].SetTarget(_stackedEnemies[i - 1].transform.position + new Vector3(0, _stackOffset, 0), _stackPoint.rotation, 5);
            }
        }
    }

    #region UI BUTTON METHODS
    /// <summary>
    /// Calls the punch animation when the button is pressed on the UI.
    /// </summary>
    public void DeliverPunch()
    {
        _animator.SetTrigger("PunchLeft");
        _isPunching = true;
        Invoke(nameof(DisablePunch), 0.3f);
    }

    private void DisablePunch()
    {
        _isPunching = false;
    }

    /// <summary>
    /// Deliver the stacked enemies inside the zone when the button is pressed on the UI.
    /// </summary>
    public void DropEnemies()
    {
        if (_insideDeliverArea)
        {
            foreach (var enemy in _stackedEnemies)
            {
                //Enemy Control Bool
                enemy.Activate = false;
                //Enables back the enemy ragdoll
                EnableRagdoll(enemy.gameObject.GetComponent<RagdollEnabler>(), true);

                //Update Game Manager 
                GameManager.PlayerCash += 10;
                GameManager.UiManager.UpdateCash();
                GameManager.UiManager.UpdateCapacity();
            }
        }
    }
    #endregion

    #region TRIGGER CHECKS
    /// <summary>
    /// Trigger enter to check the if first is colliding with a enemy and alfo check if the player is entering the deliver zone.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Check if the hand is touching the enemy
        if (other.gameObject.CompareTag(ENEMY_TAG))
        {
            //Control flag for one collision 
            if (!_collisionFlag && _isPunching)
            {
                _collisionFlag = true;

                //Enables ragdoll and apply the explosion effect
                EnableRagdoll(other.GetComponent<RagdollEnabler>());
                ReceiveHit(other.GetComponent<RagdollEnabler>(), other.ClosestPoint(transform.position));

                //Check if the player still can cary more enemies
                if (_stackedEnemies.Count < GameManager.PlayerCapacity)
                {
                    //If it is add it to the list and put it on his back after the time.
                    StartCoroutine(StackEnemy(other.GetComponent<WavingEnemy>()));
                }
                else
                {
                    _collisionFlag = false;
                }
            }
        }

        //Check if the player is entering the deliver zone
        if (other.gameObject.CompareTag(DELIVER_ZONE_TAG))
        {
            //Control Flag
            _insideDeliverArea = true;
            //Disable the button option on the UI
            GameManager.UiManager.DeliverButton.SetActive(true);
        }

    }

    /// <summary>
    /// Trigger to check if the player is leaving the Deliver Zone
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(DELIVER_ZONE_TAG))
        {
            //Control Flag
            _insideDeliverArea = false;
            //Enables the Button on the UI
            GameManager.UiManager.DeliverButton.SetActive(false);
        }
    }
    #endregion


    #region ENABLE/DISABLE ENEMY RAGDOLL
    /// <summary>
    /// Reverse the effect of the ragdoll and sets the enemy back into the animation
    /// </summary>
    /// <param name="ragdollEnabler">Ragdoll Root</param>
    IEnumerator EnableAnimator(RagdollEnabler ragdollEnabler)
    {
        yield return new WaitForSeconds(_delayAfterPunch);

        //Enable Back the Animator
        ragdollEnabler.ToggleRagdoll(false);
    }


    /// <summary>
    /// Disables the animation and enable the ragdoll of the enemy
    /// </summary>
    /// <param name="ragdollEnabler">Ragdoll Root</param>
    /// <param name="remove">True if the enemy is delivered and should be destroyed after the ragdoll effect</param>
    private void EnableRagdoll(RagdollEnabler ragdollEnabler, bool remove = false)
    {
        //Enables Ragdoll on the enemy
        ragdollEnabler.ToggleRagdoll(true);

        //If the enemy is flagged to be removed it is delivered.
        if (remove)
        {
            StartCoroutine(DestroyEnemy(ragdollEnabler.gameObject));
        }
        else
        {
            StartCoroutine(EnableAnimator(ragdollEnabler));
        }
    }

    #endregion

    #region OTHER METHODS
    /// <summary>
    /// After x Seconds adds the punched enemy to the player's back addin them to the list of enemies and updating the Game Manager.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    IEnumerator StackEnemy(WavingEnemy enemy)
    {
        yield return new WaitForSeconds(_delayAfterPunch);

        //Setting enemy
        enemy.GetComponent<Collider>().enabled = false;
        enemy.SetTarget(_stackPoint.position, Quaternion.identity, 10);

        _stackedEnemies.Add(enemy); //Add to the List of Enemies

        //Setting control booleans
        enemy.Activate = true; //Enemy Control Boolean
        _collisionFlag = false; //Controll Boolean

        //Updating the Game Manager
        GameManager.CapturedEnemies = _stackedEnemies.Count;
        GameManager.UiManager.GameScoreText.text = "Capacity:\n" + GameManager.CapturedEnemies + "/" + GameManager.PlayerCapacity;
    }

    /// <summary>
    /// Simple method to destroy the enemy after 2.5 seconds in case it was delivered by the player.
    /// </summary>
    /// <param name="obj"></param>
    IEnumerator DestroyEnemy(GameObject obj)
    {
        yield return new WaitForSeconds(2.5f);
        _stackedEnemies.Clear();
        Destroy(obj);
    }

    /// <summary>
    /// Apply explosion force to the hitted enemy
    /// </summary>
    /// <param name="ragdollEnabler">Rigidbodies of the ragdoll</param>
    /// <param name="pos">Position of the hit</param>
    private void ReceiveHit(RagdollEnabler ragdollEnabler, Vector3 pos)
    {
        ragdollEnabler.ReceiveExplosion(pos);
    }

    #endregion


}
