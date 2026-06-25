using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int money;
    public TMP_Text moneyText;

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
        Debug.Log($"돈 +{amount}, 현재 돈: {money}");
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"Money: {money}";
    }
}