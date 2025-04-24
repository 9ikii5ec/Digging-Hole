using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Ore> items;
    [SerializeField] private DiggerRuntime digger;
    [SerializeField] private int currentCells;
    [HideInInspector] public int maxCells = 3;

    [Header("Text Settings")]
    [SerializeField] private Text inventoryText;
    [SerializeField] private float animationDuration = 1.5f;
    [SerializeField] private Vector3 moveOffset = new Vector3(0, 50, 0);

    private Tween currentTween;

    private void Start()
    {
        ResetBackPuck();
    }

    public void UpdateInventoryRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, digger.digDistance))
        {
            Ore ore = hit.collider.GetComponent<Ore>();
            if (ore != null && currentCells > 0)
            {
                ore.CollectOre(this);
            }
            else if (currentCells == 0)
                ShowText("Инвентарь заполнен");
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
            Debug.Log("Нет предметов для продажи.");
            return 0;
        }
        return items[0].price;
    }


    public void AddItem(Ore _object)
    {
        if (currentCells > 0)
        {
            currentCells--;
            items.Add(_object);
        }
        else
        {
            ShowText("Инвентарь заполнен");
        }
    }

    public bool HasItems()
    {
        return items.Count > 0;
    }

    public void ShowText(string text)
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        inventoryText.text = text;
        inventoryText.gameObject.SetActive(true);

        CanvasGroup canvasGroup = inventoryText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = inventoryText.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        RectTransform rectTransform = inventoryText.rectTransform;
        Vector3 originalPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = originalPos;

        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, animationDuration * 0.3f))
           .Join(rectTransform.DOAnchorPos(originalPos + moveOffset, animationDuration))
           .AppendInterval(0.2f)
           .Append(canvasGroup.DOFade(0f, animationDuration * 0.3f))
           .OnComplete(() =>
           {
               inventoryText.gameObject.SetActive(false);
               rectTransform.anchoredPosition = originalPos;
           });

        currentTween = seq;
    }

    public void ResetBackPuck()
    {
        currentCells = maxCells;
    }
}