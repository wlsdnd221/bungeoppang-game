using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    public static AugmentManager Instance { get; private set; }
    public bool IsAugmentPanelOpen { get; private set; }        // 증강창 활성화 여부


    [Header("UI")]
    [SerializeField] private AugmentPanelUI panelUI;

    [Header("Augment Pool")]
//    [SerializeField] private List<AugmentData> allAugments = new List<AugmentData>();
    [SerializeField] private AugmentDatabase database;              // 증강목록 DB
    private List<AugmentData> AllAugments => database.augments;     // 증강목록 리스트화

    [Header("Reroll")]
    [SerializeField] private int rerollBaseCost = 10;
    [SerializeField] private int rerollCostIncrease = 10;

    private int currentRerollCost;
    
    private readonly HashSet<string> ownedAugmentIds = new();    // 이미 가지고 있는 증강 목록
    private readonly List<AugmentData> currentAugments = new();  // 현재 화면에 표시 중인 증강 목록

    private void Awake()
    {
        Instance = this;
    }

    public void ShowAugmentPanel()
    {
        IsAugmentPanelOpen = true;

        currentRerollCost = rerollBaseCost;
        panelUI.UpdateRerollCost(currentRerollCost);

        currentAugments.Clear();
        List<AugmentData> augments = GetRandomAugments(3);
        currentAugments.AddRange(augments);



        panelUI.Show(currentAugments);
        Time.timeScale = 0f;
    }

    private List<AugmentData> GetRandomAugments(int count)
    {
        List<AugmentData> candidates = new List<AugmentData>();

        foreach (AugmentData augment in AllAugments)
        {
            if (augment == null)
                continue;

            if (ownedAugmentIds.Contains(augment.augmentId))
                continue;

            candidates.Add(augment);
        }

        List<AugmentData> result = new List<AugmentData>();

        while (result.Count < count && candidates.Count > 0)
        {
            int randomIndex = Random.Range(0, candidates.Count);
            AugmentData selected = candidates[randomIndex];

            result.Add(selected);
            candidates.RemoveAt(randomIndex);
        }

        return result;
    }

    public void SelectAugment(AugmentData augment)
    {
        if (augment == null) return;

        ownedAugmentIds.Add(augment.augmentId);
        ApplyAugment(augment);

        IsAugmentPanelOpen = false;
        panelUI.Hide();

        Time.timeScale = 1f;
    }

    public void ApplyAugment(AugmentData data)
    {
        if (data == null)
        {
            Debug.LogWarning("ApplyAugment 실패: AugmentData가 null입니다.");
            return;
        }

        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("ApplyAugment 실패: PlayerStats.Instance가 없습니다.");
            return;
        }

        switch (data.effectType)
        {
            case AugmentEffectType.SellPriceMultiplier:
                PlayerStats.Instance.AddSellPriceMultiplier(data.value);
                break;

            case AugmentEffectType.CookTimeMultiplier:
                PlayerStats.Instance.AddCookTimeMultiplier(data.value);
                break;

//            case AugmentEffectType.RareChanceAdd:
//                PlayerStats.Instance.AddRareChance(data.value);
//                break;

            case AugmentEffectType.ExpMultiplier:
                PlayerStats.Instance.AddExpMultiplier(data.value);
                break;

//            case AugmentEffectType.AutoPerfectChance:
//                PlayerStats.Instance.AddAutoPerfectChance(data.value);
//                break;
        }

        Debug.Log($"증강 적용: {data.augmentName} / {data.effectType} / {data.value}");
    }

    public void RerollSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= currentAugments.Count)
        {
            Debug.LogError($"잘못된 리롤 슬롯 인덱스: {slotIndex}");
            return;
        }
        if (!MoneyManager.Instance.CanSpendMoney(currentRerollCost))
        {
            Debug.Log("돈이 부족해서 리롤할 수 없습니다.");
            return;
        }
        AugmentData newAugment = GetRandomAugmentForSlot(slotIndex);
        if (newAugment == null)
        {
            Debug.Log("리롤 가능한 증강이 없습니다.");
            return;
        }

        MoneyManager.Instance.SpendMoney(currentRerollCost);
        currentAugments[slotIndex] = newAugment;
        panelUI.RefreshSlot(slotIndex, newAugment);
        currentRerollCost += rerollCostIncrease;
        
        panelUI.UpdateRerollCost(currentRerollCost);
    }

    private AugmentData GetRandomAugmentForSlot(int slotIndex)
    {
        List<AugmentData> candidates = new List<AugmentData>();

        foreach (AugmentData augment in AllAugments)
        {
            if (augment == null)
                continue;

            if (ownedAugmentIds.Contains(augment.augmentId))
                continue;

            // 현재 리롤하는 슬롯에 있던 증강도 제외
            if (currentAugments[slotIndex] != null &&
                currentAugments[slotIndex].augmentId == augment.augmentId)
            {
                continue;
            }

            bool alreadyShownInOtherSlot = false;

            for (int i = 0; i < currentAugments.Count; i++)
            {
                if (i == slotIndex)
                    continue;

                if (currentAugments[i] != null &&
                    currentAugments[i].augmentId == augment.augmentId)
                {
                    alreadyShownInOtherSlot = true;
                    break;
                }
            }

            if (alreadyShownInOtherSlot)
                continue;

            candidates.Add(augment);
        }

        if (candidates.Count == 0)
            return null;

        int randomIndex = Random.Range(0, candidates.Count);
        return candidates[randomIndex];
    }

    // 증강 초기화
    public void ResetAugments()
    {
        ownedAugmentIds.Clear();
        currentAugments.Clear();

        currentRerollCost = rerollBaseCost;
    }
}