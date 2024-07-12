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
    public CharacterJoint[] Joints;
    public Collider[] Colliders;

    private Boolean Captured;
    private int StackPosition = 0;


    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();

    }











}