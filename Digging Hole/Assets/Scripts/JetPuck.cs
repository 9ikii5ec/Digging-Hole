using UnityEngine;

public class JetPuck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private GroundCheck groundCheck;

    [Header("Settings")]
    public float flyForce = 5f;
    public float flyEnergyCost = 1f;
    [SerializeField] private Battery battery;

    public float gravityCompensation = 9.81f;
    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (battery.energy > 0f)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fly();
            }
        }
    }

    private void Fly()
    {
        rigidbody.AddForce(Vector3.up * (flyForce + gravityCompensation), ForceMode.Acceleration);
        battery.MinusBatteryEnergy(flyEnergyCost * Time.deltaTime);
    }
}
