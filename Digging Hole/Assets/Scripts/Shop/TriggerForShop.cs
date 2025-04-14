using UnityEngine;

public class TriggerForShop : MonoBehaviour
{
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Inventory backPuck;
    [SerializeField] private Balance balance;
    [SerializeField] private GameObject ShopButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FirstPersonMovement>(out FirstPersonMovement person))
        {
            //Cursor.lockState = CursorLockMode.None;
            ShopButton.SetActive(true);

            CellItems();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        shopCanvas.gameObject.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        ShopButton.SetActive(false);
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

    public void OpenShop()
    {
        shopCanvas.gameObject.SetActive(true);
        ShopButton.SetActive(false);
    }
}
