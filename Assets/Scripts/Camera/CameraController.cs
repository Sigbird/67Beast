using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Referência ao transform do personagem
    public Transform target;

    // Distância e ângulo fixos
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Calcula a posição desejada da câmera
        Vector3 desiredPosition = target.position + offset;
        
        // Interpola suavemente entre a posição atual e a posição desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Atualiza a posição da câmera
        transform.position = smoothedPosition;

        // Mantém a câmera olhando para o alvo
        transform.LookAt(target);
    }
}
