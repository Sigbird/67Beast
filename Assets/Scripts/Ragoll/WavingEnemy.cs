using UnityEngine;

/// <summary>
/// Method to lerp the position and rotation of a enemy based on a target that is set by the punch handler when the enemy is punched
/// to create the waving follow movement of the enemy stacking.
/// </summary>
public class WavingEnemy : MonoBehaviour
{
    [Tooltip("Speed that the enemy will follow")]
    [SerializeField] private float followSpeed = 5.0f;
    [Tooltip("Speed that enemy will rotate")]
    [SerializeField] private float rotationSpeed = 5.0f;

    //Target Position set by the SetTarget
    private Vector3 targetPosition;
    //Target Rotation set by the SetTarget
    private Quaternion targetRotation;

    // bool to enable or disable the effect
    public bool Activate;

    /// <summary>
    /// Set by the Punch Handler when seding a enemy to follow some target.
    /// </summary>
    public void SetTarget(Vector3 position, Quaternion rotation, float speed)
    {
        targetPosition = position;
        targetRotation = rotation;
        followSpeed = speed;
    }

    void Update()
    {
        if (Activate)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * followSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
