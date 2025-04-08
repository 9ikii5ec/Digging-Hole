using UnityEngine;

public class TriggerForShop : MonoBehaviour
{
    [SerializeField] private Canvas shopCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<FirstPersonMovement>(out FirstPersonMovement person))
        {
            shopCanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        shopCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
