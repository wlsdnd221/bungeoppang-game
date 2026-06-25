using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public TMP_Text inventoryText;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddBungeoppang(string filling)
    {
        if (!inventory.ContainsKey(filling))
        {
            inventory[filling] = 0;
        }

        inventory[filling]++;

        UpdateUI();

        Debug.Log($"{filling} 재고 +1 / 현재 {inventory[filling]}개");
    }

    public int GetCount(string filling)
    {
        if (!inventory.ContainsKey(filling))
            return 0;

        return inventory[filling];
    }

    public bool UseBungeoppang(string filling, int amount)
    {
        if (GetCount(filling) < amount)
            return false;

        inventory[filling] -= amount;

        UpdateUI();

        return true;
    }

    void UpdateUI()
    {
        inventoryText.text = "";

        foreach (var item in inventory)
        {
            inventoryText.text += $"{item.Key}: {item.Value}\n";
        }
    }
}