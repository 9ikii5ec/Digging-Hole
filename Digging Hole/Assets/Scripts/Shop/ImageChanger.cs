using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    private Image image;

    [Header("Gradient Colors")]
    [SerializeField] private Color emptyColor = Color.yellow;
    [SerializeField] private Color fullColor = Color.green;

    [Header("BalanceMoney")]
    [SerializeField] private Balance balance;

    [Header("UpgradeCost")]
    [SerializeField] private int cost = 10;
    [SerializeField] private Text priceText;

    private void Start()
    {
        image = GetComponent<Image>();
        priceText.text = cost.ToString() + " $";
    }

    public void ImageUpdater(float value)
    {
        if (balance.money >= cost)
        {
            balance.UpdateMinusBalance(cost);
            cost += cost / 2;
            priceText.text = cost.ToString() + " $";

            image.fillAmount = Mathf.Clamp01(image.fillAmount + value);

            image.color = Color.Lerp(emptyColor, fullColor, image.fillAmount);

        }
    }
}
