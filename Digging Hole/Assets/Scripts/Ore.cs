using UnityEngine;

public class Ore : MonoBehaviour
{
    public int price;

    public void CollectOre(Inventory backPuck)
    {
        backPuck.AddItem(this);
        gameObject.SetActive(false);
    }
}
