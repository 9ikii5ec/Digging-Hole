using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [Header("Battery Properties")]
    public float energy;

    [Header("Battery Settings")]
    public float maxEnergy = 100f;
    [SerializeField] private Image image;

    private void Start()
    {
        energy = maxEnergy;
    }

    public void ChangeBatteryEnergy(float value)
    {
        energy -= value;
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        image.fillAmount = energy / maxEnergy;
    }
}
