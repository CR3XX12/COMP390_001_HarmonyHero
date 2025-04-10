using UnityEngine;

public class PlayerWorldMovement : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    private InputSystem_Actions _inputs;
    CharacterController controller;
    Animator animator;
    Vector3 moveDirection;
    private void Awake()
    {
        _inputs = new InputSystem_Actions();
        _inputs.Player.Move.performed += context => horizontalInput =  context.ReadValue<Vector2>().x;
        _inputs.Player.Move.performed += context => verticalInput =  context.ReadValue<Vector2>().y;
    }
    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        moveDirection = new Vector3(horizontalInput * moveSpeed, 0, verticalInput * moveSpeed);
        //controller.Move(moveDirection * Time.deltaTime);

        // Merge X and Z movement into a single variable (speed)
        float moveSpeedMagnitude = new Vector2(moveDirection.x, moveDirection.z).magnitude;
        animator.SetFloat("moveSpeed", moveSpeedMagnitude);
    }
}
