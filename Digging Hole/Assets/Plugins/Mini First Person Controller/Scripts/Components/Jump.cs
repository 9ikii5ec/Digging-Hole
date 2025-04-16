using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    public GroundCheck groundCheck;

    private void Reset()
    {
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            PerformJump();
        }
    }

    public void PerformJump()
    {
        rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
        Jumped?.Invoke();
    }
}
