using UnityEngine;
using YG;

public class YandexAds : MonoBehaviour
{
    [SerializeField] private Battery battery;
    [SerializeField] private YandexGame sdk;
    [SerializeField] private GameObject buttonToGetRewards;

    [Header("Авто-реклама")]
    [SerializeField] private float adInterval = 180f;
    private float timer;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewarded;
    }

    private void Start()
    {
        timer = adInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ShowInterstitial();
            timer = adInterval;
        }

        if (battery.energy <= 0.5f)
        {
            buttonToGetRewards.SetActive(true);
        }
        else
        {
            buttonToGetRewards.SetActive(false);

        }

    }

    /// <summary>
    /// Реклама без награды (автоматическая)
    /// </summary>
    private void ShowInterstitial()
    {
        sdk._FullscreenShow(); // Показывает interstitial (без ID)
    }

    /// <summary>
    /// Реклама с наградой — по желанию игрока
    /// </summary>
    public void ShowRewardedAd()
    {
        sdk._RewardedShow(1); // ID для награды (энергия)
        buttonToGetRewards.SetActive(false);
    }

    /// <summary>
    /// Получение награды за просмотр
    /// </summary>
    private void OnRewarded(int id)
    {
        if (id == 1)
        {
            battery.PlusBatteryEnergy(battery.maxEnergy - battery.energy);
        }
    }
}
