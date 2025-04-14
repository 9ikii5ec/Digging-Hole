using EasyMobileInput;
using UnityEngine;

public class MobileMovement : MonoBehaviour
{
    [SerializeField] private JetPuck jetpack;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private float moveSpeed = 5f;

    private bool isJumping = false;

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
    }

    private void OnJumpReleased()
    {
        isJumping = false;
    }

    private void Update()
    {
        transform.position += movementJoystick.CurrentProcessedValue * Time.deltaTime * moveSpeed;

        if (isJumping)
        {
            jetpack.Fly();
        }
    }
}
