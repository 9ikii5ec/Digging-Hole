using UnityEngine;

public class LookAtPlayerUI : MonoBehaviour
{
    public Transform playerCamera;

    private void Update()
    {
        transform.LookAt(playerCamera);
        transform.rotation = Quaternion.LookRotation(playerCamera.forward);
    }
}

