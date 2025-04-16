using UnityEngine;

public class JetPuck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private GroundCheck groundCheck;

    [Header("Settings")]
    public float flyForce = 5f;

    [Header("Flight Control")]
    public float gravityCompensation = 9.81f;
    public float maxFlySpeed = 5f;

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            Fly();
    }

    public void Fly()
    {
        if (rigidbody.velocity.y < maxFlySpeed)
        {
            rigidbody.AddForce(Vector3.up * (flyForce + gravityCompensation), ForceMode.Acceleration);
        }
    }
}
