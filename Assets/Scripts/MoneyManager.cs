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
}