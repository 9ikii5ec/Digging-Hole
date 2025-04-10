using UnityEngine;

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

    public void UpgradeJetPack(int value)
    {
        if (balance.money >= jetpackImage.cost && jetpackImage.image.fillAmount < 1f)
        {
            jetpack.flyForce += value;
            jetpack.flyEnergyCost += value;
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
}
