using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Referências necessárias
    public Animator animator;
    public float moveSpeed = 5f;  // Velocidade de movimento do personagem
    public float jumpForce = 10f; // Força do salto

    public Collider[] hands;

    // Variáveis de controle de movimento
    private Rigidbody rb;
    private Vector3 moveInput;

    // Variáveis de controle de animação
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Captura dos inputs do joystick (Mobile)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.B)) Punch();

        // Calcula o vetor de movimento baseado nos inputs
        moveInput = new Vector3(vertical, 0f, -horizontal).normalized;

        // Verifica se o jogador pressionou o botão de salto
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Atualiza a animação baseada nos inputs de movimento
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        // Move o personagem usando o Rigidbody e os inputs
        MoveCharacter();
    }

    void MoveCharacter()
    {
        // Calcula o movimento
        Vector3 movement = moveInput * moveSpeed * Time.fixedDeltaTime;

        // Move o personagem
        rb.MovePosition(transform.position + movement);

        // Rotaciona o personagem na direção do movimento
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * moveSpeed));
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            moveSpeed = 3;
        }
    }

    void UpdateAnimator()
    {
        // Atualiza os parâmetros do Animator
        if (isJumping)
        {
            animator.SetFloat("MoveSpeed", 0);
        }
        else
        {
            animator.SetFloat("MoveSpeed", moveInput.magnitude);
        }

        animator.SetBool("IsJumping", isJumping);
    }

    void Punch()
    {
        animator.SetTrigger("Punch");
        Invoke(nameof(EnableFirsts), 0.3f);
    }

    void EnableFirsts()
    {
        hands[0].enabled = true;
        hands[1].enabled = true;
        Invoke(nameof(DisableFirsts), 0.1f);
    }

    void DisableFirsts()
    {
        hands[0].enabled = false;
        hands[1].enabled = false;
    }

    // Callback quando o personagem colide com o chão
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            moveSpeed = 7;
        }
    }
}
