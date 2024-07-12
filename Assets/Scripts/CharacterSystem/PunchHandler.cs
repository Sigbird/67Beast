using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEditor;
using UnityEngine;

public class PunchHandler : MonoBehaviour
{

    public Transform leftHand;
    public Transform rightHand;
    public float punchRange = 1.0f;
    public Transform stackPoint; // The point on the player's back where enemies will be stacked
    public float stackOffset = 1.0f; // Offset for stacking enemies

    public List<WavingEnemy> stackedEnemies = new List<WavingEnemy>();

    public Animator animator;

    private bool colliderTrigger;

    private bool insideDeliverArea;

    void Update()
    {
        UpdateStackedEnemies();
    }

    public void DeliverPunch()
    {
        animator.SetTrigger("PunchLeft");
    }

    public void DeliverEnemies()
    {
        if (insideDeliverArea)
        {
            foreach (var item in stackedEnemies)
            {
                item.Activate = false;
                EnableRagdoll(item.gameObject.GetComponent<RagdollEnabler>(), true);
                GameManager.PlayerCash += 10;
                GameManager.uiManager.UpdateCash();
                GameManager.uiManager.UpdateCapacity();
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!colliderTrigger)
            {
                colliderTrigger = true;

                EnableRagdoll(other.GetComponent<RagdollEnabler>());
                ReceiveHit(other.GetComponent<RagdollEnabler>(), other.ClosestPoint(transform.position));

                if (stackedEnemies.Count < GameManager.PlayerCapacity)
                {
                    StartCoroutine(StackEnemy(other.GetComponent<WavingEnemy>()));
                }
                else
                {
                    colliderTrigger = false;
                }
            }
        }

        if (other.gameObject.CompareTag("DeliverZone"))
        {
            insideDeliverArea = true;
            GameManager.uiManager.DeliverButton.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("DeliverZone"))
        {
            insideDeliverArea = false;
            GameManager.uiManager.DeliverButton.SetActive(false);
        }
    }




    public void EnableRagdoll(RagdollEnabler ragdollEnabler, bool Remove = false)
    {
        ragdollEnabler.Animator.enabled = false;
        foreach (CharacterJoint joint in ragdollEnabler.Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in ragdollEnabler.Colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in ragdollEnabler.Rigidbodies)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
        if (Remove)
        {
            StartCoroutine(DestroyEnemy(ragdollEnabler.gameObject));
        }
        else
        {
            StartCoroutine(EnableAnimator(ragdollEnabler));
        }

    }

    IEnumerator DestroyEnemy(GameObject obj)
    {
        yield return new WaitForSeconds(2.5f);
        stackedEnemies.Clear();
        Destroy(obj);
    }



    IEnumerator EnableAnimator(RagdollEnabler ragdollEnabler)
    {
        yield return new WaitForSeconds(3f);
        ragdollEnabler.Animator.enabled = true;
        foreach (CharacterJoint joint in ragdollEnabler.Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Collider collider in ragdollEnabler.Colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in ragdollEnabler.Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }

    void ReceiveHit(RagdollEnabler ragdollEnabler, Vector3 pos)
    {
        foreach (Rigidbody rigidbody in ragdollEnabler.Rigidbodies)
        {
            rigidbody.AddExplosionForce(1000, pos, 100, 1);
        }
    }

    IEnumerator StackEnemy(WavingEnemy enemy)
    {
        yield return new WaitForSeconds(3f);
        enemy.GetComponent<Collider>().enabled = false;
        Vector3 newPosition = stackPoint.position;
        enemy.SetTarget(newPosition, Quaternion.identity, 10);
        enemy.transform.rotation = stackPoint.rotation;
        stackedEnemies.Add(enemy);
        enemy.Activate = true;
        colliderTrigger = false;
        GameManager.CapturedEnemies = stackedEnemies.Count;
        GameManager.uiManager.GameScoreText.text = "Capacity:\n" + GameManager.CapturedEnemies + "/" + GameManager.PlayerCapacity;
    }

    void UpdateStackedEnemies()
    {
        for (int i = 0; i < stackedEnemies.Count; i++)
        {
            WavingEnemy enemy = stackedEnemies[i];

            if (i == 0)
            {
                Vector3 newPosition = stackPoint.position + new Vector3(0, stackOffset, 0);
                enemy.SetTarget(newPosition, stackPoint.rotation, 1000);
            }
            else
            {
                Vector3 newPosition = stackedEnemies[i - 1].transform.position + new Vector3(0, stackOffset, 0);
                enemy.SetTarget(newPosition, stackPoint.rotation, 5);
            }
        }
    }
}
