using UnityEngine;

public class StandManager : MonoBehaviour
{
    public static StandManager Instance { get; private set; }

    [Header("가판대 설정")]
    [SerializeField] private Transform standParent;
    [SerializeField] private GameObject bungeoppangPrefab;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(500f, 180f);

    private void Awake()
    {
        Instance = this;
    }

    public void AddBungeoppang()
    {
        GameObject obj = Instantiate(bungeoppangPrefab, standParent);

        RectTransform rect = obj.GetComponent<RectTransform>();

        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float y = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        rect.anchoredPosition = new Vector2(x, y);
        rect.localRotation = Quaternion.Euler(0, 0, Random.Range(-15, 15));
    }

    // 판매 처리
    public void Sell(SellableBungeoppang bungeoppang)
    {
        int basePrice = bungeoppang.Price;  // 기본 판매금
        float multiplier = 1f;              // 배율

        if (PlayerStats.Instance != null)
        {
            multiplier = PlayerStats.Instance.sellPriceMultiplier;
        }

        int finalPrice = Mathf.RoundToInt(basePrice * multiplier);

        MoneyManager.Instance.AddMoney(finalPrice);
        DayManager.Instance.RecordSale(finalPrice, true);

        Destroy(bungeoppang.gameObject);
    }

    // 초기화
    public void ResetStand()
    {
        for (int i = standParent.childCount - 1; i >= 0; i--)
        {
            Destroy(standParent.GetChild(i).gameObject);
        }

        Debug.Log("가판대 초기화 완료");
    }
}