using UnityEngine;

/// <summary>
/// Component with the references to the ragdoll of the enemies, and implementing the rigidbody methods to enable and disable them.
/// </summary>
public class RagdollEnabler : MonoBehaviour
{
    [Tooltip("Ragdoll root of the enemy")]
    [SerializeField] private Transform _ragdollRoot;
    private Rigidbody[] _rigidbodies;
    private CharacterJoint[] _joints;
    private Collider[] _colliders;
    private Animator _animator;

    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _rigidbodies = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        _joints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        _colliders = _ragdollRoot.GetComponentsInChildren<Collider>();
    }

    /// <summary>
    /// Toggles the Ragdoll called by the Punch Handler of the player
    /// </summary>
    /// <param name="toggle">True will enable the ragdoll of the character</param>
    public void ToggleRagdoll(bool toggle)
    {
        //Disable the animator
        _animator.enabled = !toggle;

        //Enable Joints
        foreach (CharacterJoint joint in _joints)
        {
            joint.enableCollision = toggle;
        }

        //Enable Collider
        foreach (Collider collider in _colliders)
        {
            collider.enabled = toggle;
        }

        //Enables the gravity and detectCollisions
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.detectCollisions = toggle;
            rigidbody.useGravity = toggle;
        }
    }

    /// <summary>
    /// Simulates the explosion when called by the Punch Handler
    /// </summary>
    /// <param name="pos"></param>
    public void ReceiveExplosion(Vector3 pos)
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.AddExplosionForce(1000, pos, 100, 1);
        }
    }
}