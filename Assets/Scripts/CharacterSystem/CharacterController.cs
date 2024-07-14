using UnityEngine;

/// <summary>
/// Base character controller of the player character, implementing the basic movement, animation of movement and collision check with the ground.
/// </summary>
public class CharacterController : MonoBehaviour
{
    // References
    [Tooltip("Base movespeed of the player character")]
    [SerializeField] private float _moveSpeed = 5f;  
    [Tooltip("Reference of the hand colliders of the character")]
    [SerializeField] private Collider[] _hands;
    [Tooltip("Reference to the material of the player character")]
    [SerializeField] private SkinnedMeshRenderer _playerMaterial;
    private Animator _animator;
    private JoystickHandler _joystick;

    // Control movement variables
    private Rigidbody rb;
    private Vector3 moveInput;

    #region INITIALIZERS
    private void Start()
    {
        _joystick = GameObject.Find("Ui_Img_JoystickBackground").GetComponent<JoystickHandler>();
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = new Vector3(-_joystick.Vertical(), 0f, _joystick.Horizontal()).normalized;

        UpdateAnimator();
        MoveCharacter();
    }
    #endregion

    #region PLAYER MOVEMENT
    /// <summary>
    /// Move the character in the world using the move input gathered on the joysticks;
    /// </summary>
    private void MoveCharacter()
    {
        rb.MovePosition(transform.position + (moveInput * _moveSpeed * Time.fixedDeltaTime));

        if (moveInput != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveInput), Time.fixedDeltaTime * _moveSpeed));
        }
    }

    /// <summary>
    /// Changes the attribute MoveSpeed from the animator.
    /// </summary>
    private void UpdateAnimator()
    {
        _animator.SetFloat("MoveSpeed", moveInput.magnitude);
    }
    #endregion

    #region COLLIDERS/TRIGGERS
    /// <summary>
    /// Callback when the player collides with the floor.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _moveSpeed = 7;
        }
    }
    #endregion

    #region OTHER METHODS
    /// <summary>
    /// Method to change the collor of the player.
    /// </summary>
    /// <param name="newColor"></param>
    public void ChangePlayerColor(Color newColor)
    {
        _playerMaterial.materials[0].color = newColor;
    }
    #endregion
}
