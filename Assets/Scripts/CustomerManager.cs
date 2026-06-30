using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{

    public TMP_Text orderText;
    public Image satisfactionFill;

    private InventoryManager inventoryManager;
    private MoneyManager moneyManager;

    private string orderFilling;
    private int orderAmount;

    private float satisfaction;
    private float maxSatisfaction = 100f;
    private float decreasePerSecond = 8f;

    private string[] fillings =
    {
        "RedBean",
        "Custard"
    };

    void Awake()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        moneyManager = FindFirstObjectByType<MoneyManager>();
    }

    void Start()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        if (DayManager.Instance == null || !DayManager.Instance.IsOpen()) {
            return;
        }

        UpdateSatisfaction();
        TryAutoSell();
        UpdateUI();
    }

    void GenerateNewOrder()
    {
        orderFilling = fillings[Random.Range(0, fillings.Length)];
        orderAmount = Random.Range(1, 3);
        satisfaction = maxSatisfaction;
    }

    void UpdateSatisfaction()
    {
        satisfaction -= decreasePerSecond * Time.deltaTime;

        if (satisfaction <= 0)
        {
            Debug.Log("손님이 떠남");
            GenerateNewOrder();
        }
    }

    void TryAutoSell()
    {
        if (inventoryManager.UseBungeoppang(orderFilling, orderAmount))
        {
            int basePrice = GetPrice(orderFilling) * orderAmount;
            int finalPrice = CalculateFinalPrice(basePrice);

            moneyManager.AddMoney(finalPrice);                   // 판매금액 추가
            DayManager.Instance.RecordSale(finalPrice, true);    // 하루매출 기록

            Debug.Log($"판매 성공: {orderFilling} x{orderAmount}");
            GenerateNewOrder();
        }
    }

    int GetPrice(string filling)
    {
        if (filling == "RedBean") return 1000;
        if (filling == "Custard") return 1200;

        return 1000;
    }

    int CalculateFinalPrice(int basePrice)
    {
        if (satisfaction >= 80f)
            return Mathf.RoundToInt(basePrice * 1.5f);

        if (satisfaction >= 50f)
            return basePrice;

        if (satisfaction >= 20f)
            return Mathf.RoundToInt(basePrice * 0.8f);

        return Mathf.RoundToInt(basePrice * 0.5f);
    }

    void UpdateUI()
    {
        if (orderText == null)
            return;

        orderText.text =
            $"Order: {orderFilling} x{orderAmount}\n" +
            $"Satisfaction: {Mathf.CeilToInt(satisfaction)}%";

        float ratio = satisfaction / maxSatisfaction;

        satisfactionFill.fillAmount = ratio;

        if (ratio > 0.6f)
            satisfactionFill.color = Color.green;
        else if (ratio > 0.3f)
            satisfactionFill.color = Color.yellow;
        else
            satisfactionFill.color = Color.red;
    }

//    void UpdateUI()
//    {
//        orderText.text =
//            $"Order: {orderFilling} x{orderAmount}\n" +
//            $"Satisfaction: {Mathf.CeilToInt(satisfaction)}%";
//
//        satisfactionFill.fillAmount = satisfaction / maxSatisfaction;
//    }
}