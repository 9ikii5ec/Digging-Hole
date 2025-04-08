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

    public void UpdateBalance(int value)
    {
        money -= value;
        balance.text = money.ToString() + " $";
    }
}
