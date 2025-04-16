using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [Header("Battery Properties")]
    public float energy;

    [Header("Battery Settings")]
    public float maxEnergy = 100f;

    [Header("BatteryImageSettings")]
    [SerializeField] private Image image;
    [SerializeField] private Sprite lowBattery;
    [SerializeField] private Sprite lowMiddleBattery;
    [SerializeField] private Sprite heightMiddleBattery;
    [SerializeField] private Sprite heightBattery;

    private void Start()
    {
        energy = maxEnergy;
        UpdateBatteryImage();
    }

    public void MinusBatteryEnergy(float value)
    {
        energy -= value;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        UpdateBatteryImage();
    }

    public void PlusBatteryEnergy(float value)
    {
        energy += value;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        UpdateBatteryImage();
    }

    private void UpdateBatteryImage()
    {
        float percent = energy / maxEnergy;

        if (percent > 0.7f)
        {
            image.sprite = heightBattery;
        }
        else if (percent > 0.6f)
        {
            image.sprite = heightMiddleBattery;
        }
        else if (percent > 0.5f)
        {
            image.sprite = lowMiddleBattery;
        }
        else
        {
            image.sprite = lowBattery;
        }
    }
}
