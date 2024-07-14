using UnityEngine;

/// <summary>
/// Basic camera controller script to follow the player keeping a fixed angle and distance.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Player transform reference.")]
    [SerializeField] private Transform target;
    [Tooltip("Distance Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [Tooltip("Camera Smootness")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        // Lerp its position to follow the target
        transform.position = Vector3.Lerp(transform.position, (target.position + offset), smoothSpeed);

        // Keep focused on the Target
        transform.LookAt(target);
    }
}
