using UnityEngine;

public class PlayerWorldMovement : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;

    CharacterController controller;
    Animator animator;
    Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Movement in X-axis
        verticalInput = Input.GetAxis("Vertical"); // Movement in Z-axis

        FlipSprite();
    }

    void FixedUpdate()
    {
        moveDirection = new Vector3(horizontalInput * moveSpeed, 0, verticalInput * moveSpeed);
        controller.Move(moveDirection * Time.deltaTime);

        // Merge X and Z movement into a single variable (speed)
        float moveSpeedMagnitude = new Vector2(moveDirection.x, moveDirection.z).magnitude;
        animator.SetFloat("moveSpeed", moveSpeedMagnitude);
    }

    void FlipSprite()
    {
        if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
