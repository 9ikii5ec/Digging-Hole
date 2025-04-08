using UnityEngine;

public class UpgradeTools : MonoBehaviour
{
    [SerializeField] private JetPuck jetpack;
    [SerializeField] private Battery battery;
    [SerializeField] private DiggerRuntime digger;
    [SerializeField] private Balance balance;
    [SerializeField] private Inventory backPuck;

    public void UpgradeJetPack(int value)
    {
        jetpack.flyForce += value;
        jetpack.flyEnergyCost += value;
    }

    public void UpgradeBattery(int value)
    {
        battery.maxEnergy += value;
    }

    public void FullBattery()
    {
        battery.energy = battery.maxEnergy;
    }

    public void UpgardeDiggerSize(float value)
    {
        digger.size += value;
    }

    public void UpgardeBackPuck(int value = 5)
    {
        backPuck.maxCells += value;
    }
}
