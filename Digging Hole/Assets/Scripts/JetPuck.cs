using UnityEngine;

public class JetPuck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private GroundCheck groundCheck;

    [Header("Settings")]
    [SerializeField] private Battery battery;
    [SerializeField] private float flyForce = 5f;
    [SerializeField] private float flyEnergyCost = 1f;

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!groundCheck.isGrounded && battery.energy > 0f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fly();
            }
        }
    }

    private void Fly()
    {
        rigidbody.AddForce(Vector3.up * flyForce, ForceMode.Acceleration);
        battery.ChangeBatteryEnergy(flyEnergyCost * Time.deltaTime);
    }
}
