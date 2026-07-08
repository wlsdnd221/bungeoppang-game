using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public int money;
    public TMP_Text moneyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
        Debug.Log($"돈 +{amount}, 현재 돈: {money}");
    }

    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"Money: {money}";
    }

    // 소지금이 해당 비용보다 큰지 여부
    public bool CanSpendMoney(int amount)
    {
        return money >= amount;
    }

    // 소비
    public void SpendMoney(int amount)
    {
        if (!CanSpendMoney(amount))
            return;

        money -= amount;
        UpdateUI();

        Debug.Log($"돈 -{amount}, 현재 돈: {money}");
    }

    // 현재 돈
    public int GetMoney()
    {
        return money;
    }
}