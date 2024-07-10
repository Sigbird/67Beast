using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using DG.Tweening;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField]
    public Animator Animator;
    [SerializeField]
    public Transform RagdollRoot;
    public Transform PlayerBag;
    [SerializeField]
    public bool StartRagdoll = false;
    // Only public for Ragdoll Runtime GUI for explosive force
    public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    private Boolean Captured;

    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();

        if (StartRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) EnableRagdoll();

        if (Input.GetKeyDown(KeyCode.O)) EnableAnimator();

        if (Captured) transform.position = PlayerBag.transform.position;
    }

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            EnableRagdoll();
            ReceiveHit(other.ClosestPoint(transform.position));
        }
    }

    void ReceiveHit(Vector3 Pos)
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.AddExplosionForce(1000, Pos, 100, 1);

            StartCoroutine(MoveToPlayerBag(rigidbody));
        }
    }

    IEnumerator MoveToPlayerBag(Rigidbody rb)
    {
        yield return new WaitForSeconds(1f);
        // Calculate the offset from the current position to the target position
        Vector3 offset = PlayerBag.position - rb.position;
        this.transform.SetParent(PlayerBag);
        // Use DoTween to move the Rigidbody to the target position with the specified offset
        rb.DOMove(Vector3.zero, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
    {
        EnableAnimator();
        transform.eulerAngles = new Vector3(0, 90, 0);

    });

    }
}