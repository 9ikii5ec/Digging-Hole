using UnityEngine;

public class TriggerForShop : MonoBehaviour
{
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Inventory backPuck;
    [SerializeField] private Balance balance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FirstPersonMovement>(out FirstPersonMovement person))
        {
            shopCanvas.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            CellItems();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        shopCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CellItems()
    {
        while (backPuck.HasItems())
        {
            balance.UpdatePlusBalance(backPuck.GetOrePrice());
            backPuck.SellOneItem();
            backPuck.ResetBackPuck();
        }
    }
}
