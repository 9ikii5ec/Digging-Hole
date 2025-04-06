using UnityEngine;

public class ShovelDirection : MonoBehaviour
{
    [Header("Camera to follow")]
    public Camera targetCamera;

    [Header("Rotation settings")]
    public float rotationSpeed = 10f;

    private void Update()
    {
        Vector3 direction = targetCamera.transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
