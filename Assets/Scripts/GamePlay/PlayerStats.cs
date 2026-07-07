using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }


    [Header("Cooking")]
    public float cookTimeMultiplier = 1f;

    [Header("Money")]
    public float sellPriceMultiplier = 1f;

    [Header("Exp")]
    public int expBonus = 0;
   public float expMultiplier = 1f;

//    [Header("Special Fish")]
//    public float rareChanceAdd = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddSellPriceMultiplier(float value)
    {
        sellPriceMultiplier += value;
    }

    public void AddCookTimeMultiplier(float value)
    {
        cookTimeMultiplier += value;
    }

//    public void AddRareChance(float value)
//    {
//        rareChanceAdd += value;
//    }

    public void AddExpBonus(int value)
    {
        expBonus += value;
    }

    public void AddExpMultiplier(float value)
    {
        expMultiplier += value;
    }

    public void ResetStats()
    {
        sellPriceMultiplier = 1f;
        cookTimeMultiplier = 1f;
        expBonus = 0;
        expMultiplier = 1f;

        Debug.Log("PlayerStats 초기화 완료");
    }

}
