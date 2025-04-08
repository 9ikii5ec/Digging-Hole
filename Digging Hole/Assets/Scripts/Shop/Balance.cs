using UnityEngine;
using UnityEngine.UI;

public class Balance : MonoBehaviour
{
    private Text balance;
    public int money = 100;

    private void Start()
    {
        balance = GetComponent<Text>();
        UpdateBalance();
    }

    public void UpdateBalance()
    {
        balance.text = money.ToString() + " $";
    }

    public void UpdateMinusBalance(int value)
    {
        if (money >= value)
        {
            money -= value;
            balance.text = money.ToString() + " $";
        }
    }

    public void UpdatePlusBalance(int value)
    {
        money += value;
        balance.text = money.ToString() + " $";
    }
}
