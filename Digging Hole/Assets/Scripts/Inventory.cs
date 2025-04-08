using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Ore> items;
    [SerializeField] private DiggerRuntime digger;
    [SerializeField] private int currentCells;
    [HideInInspector] public int maxCells = 5;

    private void Start()
    {
        currentCells = maxCells;
    }

    public void UpdateInventoryRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, digger.digDistance))
        {
            Ore ore = hit.collider.GetComponent<Ore>();
            if (ore != null)
            {
                ore.CollectOre(this);
            }
        }
    }

    public void SellOneItem()
    {
        if (items.Count == 0)
        {
            Debug.Log("Инвентарь пуст.");
            return;
        }

        Ore itemToSell = items[0];
        items.RemoveAt(0);

        Destroy(itemToSell.gameObject);
        Debug.Log("Предмет продан: " + itemToSell.name);
    }


    public int GetOrePrice()
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("Нет предметов для продажи.");
            return 0;
        }
        return items[0].price;
    }


    public void AddItem(Ore _object)
    {
        items.Add(_object);
    }

    public bool HasItems()
    {
        return items.Count > 0;
    }
}