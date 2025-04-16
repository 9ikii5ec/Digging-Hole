using UnityEngine;
using UnityEngine.UI;

public class UpgradeTools : MonoBehaviour
{
    [SerializeField] private JetPuck jetpack;
    [SerializeField] private Battery battery;
    [SerializeField] private DiggerRuntime digger;
    [SerializeField] private Inventory backPuck;
    [SerializeField] private Balance balance;

    [Header("Tools Image")]
    [SerializeField] private ImageChanger jetpackImage;
    [SerializeField] private ImageChanger batteryImage;
    [SerializeField] private ImageChanger diggerImage;
    [SerializeField] private ImageChanger backPuckImage;

    [Header("LampSettings")]
    [SerializeField] private GameObject lamp;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private Text lampText;
    private int lampCount;

    public void UpgradeJetPack(int value)
    {
        if (balance.money >= jetpackImage.cost && jetpackImage.image.fillAmount < 1f)
        {
            jetpack.flyForce += value;
        }
    }

    public void UpgradeBattery(int value)
    {
        if (balance.money >= batteryImage.cost && batteryImage.image.fillAmount < 1f)
        {
            battery.maxEnergy += value;
            battery.energy += value;
        }
    }

    public void FullBattery()
    {
        float value = battery.maxEnergy - battery.energy;
        if (balance.money >= 15)
            battery.PlusBatteryEnergy(value);
    }

    public void UpgardeDiggerSize(float value)
    {
        if (balance.money >= diggerImage.cost && diggerImage.image.fillAmount < 1f)
        {
            digger.defaultSize += value;
        }
    }

    public void UpgardeBackPuck(int value = 5)
    {
        if (balance.money >= backPuckImage.cost && backPuckImage.image.fillAmount < 1f)
        {
            backPuck.maxCells += value;
            backPuck.ResetBackPuck();
        }
    }

    public void BuyLamp()
    {
        if (balance.money >= 10)
        {
            lampCount++;
            lampText.text = "Lamp: " + lampCount.ToString();
            Debug.Log("Buy Lamp");
        }

    }

    public void PlaceLamp()
    {
        if (lampCount > 0)
        {
            Debug.Log("Place Lamp");

            lampCount--;
            lampText.text = "Lamp: " + lampCount.ToString();

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * 0.1f;
                Quaternion rotation = Quaternion.Euler(0, 180, 0);

                Instantiate(lamp, spawnPosition, rotation);
            }
        }
    }
}
