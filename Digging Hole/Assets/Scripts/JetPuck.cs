using UnityEngine;

public class JetPuck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private GroundCheck groundCheck;

    [Header("Settings")]
    public float flyForce = 5f;
    public float flyEnergyCost = 1f;
    [SerializeField] private Battery battery;

    [Header("Flight Control")]
    public float gravityCompensation = 9.81f;
    public float maxFlySpeed = 5f;

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Fly();
    }

    public void Fly()
    {
        if (battery.energy > 0f)
        {
            if (rigidbody.velocity.y < maxFlySpeed)
            {
                rigidbody.AddForce(Vector3.up * (flyForce + gravityCompensation), ForceMode.Acceleration);
                battery.MinusBatteryEnergy(flyEnergyCost * Time.deltaTime);
            }
        }
    }
}
