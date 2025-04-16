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

        // ������ ������ ���� ��� ��� ������� ������
        if (!hasJumped && jump != null && (!jump.groundCheck || jump.groundCheck.isGrounded))
        {
            jump.PerformJump();
            hasJumped = true;
        }
    }

    private void OnJumpReleased()
    {
        isJumping = false;
        hasJumped = false; // ��������� ������ ��� ��������� �������
    }

    private void FixedUpdate()
    {
        transform.position += movementJoystick.CurrentProcessedValue * Time.deltaTime * moveSpeed;

        // ���� ����� ������ ������ � ���������� ����
        if (isJumping)
        {
            jetpack.Fly();
        }
    }
}
