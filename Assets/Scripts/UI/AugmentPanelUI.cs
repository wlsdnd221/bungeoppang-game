using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AugmentPanelUI : MonoBehaviour
{
    [Header("Augment Slots")]
    [SerializeField] private AugmentSlotUI slot1;
    [SerializeField] private AugmentSlotUI slot2;
    [SerializeField] private AugmentSlotUI slot3;

    [SerializeField] private TMP_Text rerollCostText;
//    [SerializeField] private TMP_Text rerollCostText;

    private AugmentSlotUI[] slots;

    private void Awake()
    {
        slots = new AugmentSlotUI[]
        {
            slot1,
            slot2,
            slot3
        };

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotIndex(i);
        }
    }

    private void InitSlots()
    {
        if (slots != null)
            return;

        slots = new AugmentSlotUI[]
        {
            slot1,
            slot2,
            slot3
        };

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotIndex(i);
        }
    }

    public void Show(List<AugmentData> augments)
    {
        InitSlots();

        if (augments.Count < slots.Length)
        {
            Debug.LogError("증강 데이터가 부족합니다.");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetData(augments[i]);
        }

        gameObject.SetActive(true);
    }

    public void RefreshSlot(int slotIndex, AugmentData augment)
    {
        InitSlots();
        
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError($"잘못된 슬롯 번호 : {slotIndex}");
            return;
        }

        slots[slotIndex].SetData(augment);
    }

    public void UpdateRerollCost(int cost)
    {
        if (rerollCostText != null)
        {
            rerollCostText.text = $"리롤 비용 : {cost}G";
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}