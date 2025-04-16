using EasyMobileInput;
using UnityEngine;

public class MobileMovement : MonoBehaviour
{
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private JetPuck jetpack;

    private bool isJumping = false;
    private bool hasJumped = false;

    private Jump jump;

    private void Awake()
    {
        jump = GetComponent<Jump>();
    }

    private void OnEnable()
    {
        jumpButton.OnPressed += OnJumpPressed;
        jumpButton.OnReleased += OnJumpReleased;
    }

    private void OnDisable()
    {
        jumpButton.OnPressed -= OnJumpPressed;
        jumpButton.OnReleased -= OnJumpReleased;
    }

    private void OnJumpPressed()
    {
        isJumping = true;

        // Прыжок только один раз при нажатии кнопки
        if (!hasJumped && jump != null && (!jump.groundCheck || jump.groundCheck.isGrounded))
        {
            jump.PerformJump();
            hasJumped = true;
        }
    }

    private void OnJumpReleased()
    {
        isJumping = false;
        hasJumped = false; // Разрешаем прыжок при следующем нажатии
    }

    private void FixedUpdate()
    {
        transform.position += movementJoystick.CurrentProcessedValue * Time.deltaTime * moveSpeed;

        // Если игрок держит кнопку — активируем полёт
        if (isJumping)
        {
            jetpack.Fly();
        }
    }
}
