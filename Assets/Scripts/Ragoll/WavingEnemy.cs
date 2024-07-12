using UnityEngine;

public class WavingEnemy : MonoBehaviour
{
    public float followSpeed = 5.0f;
    public float rotationSpeed = 5.0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public bool Activate;

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
