using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }


    [SerializeField] private SkillDatabase skillDatabase;                    // 스킬 DB
    private List<SkillData> AllSkills => skillDatabase.GetAllSkills();     // 스킬목록 리스트화

    private Dictionary<SkillData, int> skillLevels = new Dictionary<SkillData, int>();

   private void Awake()
   {
       if (Instance != null && Instance != this)
       {
           Destroy(gameObject);
           return;
       }

       Instance = this;

       foreach (SkillData skill in skillDatabase.GetAllSkills())
       {
           skillLevels[skill] = 0;
           Debug.Log($"SkillManager 등록: {skill.skillName}");
       }
   }

    public int GetSkillLevel(SkillData skill)
    {
        if (skill == null)
        {
            Debug.LogError("GetSkillLevel: skill is null");
            return 0;
        }

        if (!skillLevels.ContainsKey(skill))
        {
            Debug.LogError($"GetSkillLevel: 등록되지 않은 스킬입니다 - {skill.skillName}", skill);
            return 0;
        }

        return skillLevels[skill];
    }

    public bool IsMaxLevel(SkillData skill)
    {
        return GetSkillLevel(skill) >= skill.maxLevel;
    }

    public int GetUpgradeCost(SkillData skill)
    {
        int currentLevel = GetSkillLevel(skill);
        return Mathf.RoundToInt(skill.baseCost * Mathf.Pow(skill.costMultiplier, currentLevel));
    }

    public bool IsUnlocked(SkillData skill)
    {
        switch (skill.unlockType)
        {
            case SkillUnlockType.None:
                return true;

            case SkillUnlockType.TotalCollected:
                return PlayerStats.Instance.TotalCollectedBungeoppang >= skill.unlockRequiredAmount;

            case SkillUnlockType.TotalSpecialCollected:
                return PlayerStats.Instance.TotalSpecialBungeoppang >= skill.unlockRequiredAmount;
        }

        return false;
    }

    public bool HasPrerequisites(SkillData skill)
    {
        if (skill.prerequisiteSkills == null || skill.prerequisiteSkills.Count == 0)
        {
            return true;
        }

        foreach (SkillData prerequisite in skill.prerequisiteSkills)
        {
            if (GetSkillLevel(prerequisite) <= 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanUpgrade(SkillData skill)
    {
        if (!IsUnlocked(skill))
            return false;

        if (!HasPrerequisites(skill))
            return false;

        int currentLevel = GetSkillLevel(skill);

        if (currentLevel >= skill.maxLevel)
            return false;

        int cost = GetUpgradeCost(skill);

        return MoneyManager.Instance.CanSpendMoney(cost);
    }

    public void UpgradeSkill(SkillData skill)
    {
        if (!CanUpgrade(skill))
        {
            Debug.Log("스킬 구매 불가");
            return;
        }

        int cost = GetUpgradeCost(skill);

        MoneyManager.Instance.SpendMoney(cost);
        skillLevels[skill]++;

        RecalculatePlayerStats();

        Debug.Log($"{skill.skillName} 구매 완료 Lv.{skillLevels[skill]}");
    }

    public float GetSkillValue(SkillType type)
    {
        float total = 0f;

        foreach (SkillData skill in AllSkills)
        {
            if (skill.skillType != type)
                continue;

            int level = GetSkillLevel(skill);
            total += skill.valuePerLevel * level;
        }

        return total;
    }

    public void RecalculatePlayerStats()
    {
        PlayerStats.Instance.ResetSkillStats();

        foreach (SkillData skill in skillDatabase.GetAllSkills())
        {
            int level = GetSkillLevel(skill);

            if (level <= 0)
                continue;

            ApplySkill(skill, level);
        }

        Debug.Log("스킬 능력치 재계산 완료");
    }

    private void ApplySkill(SkillData skill, int level)
    {
        float value = skill.valuePerLevel * level;

        switch (skill.skillType)
        {
            case SkillType.CookSpeedUp:
                PlayerStats.Instance.AddCookTimeMultiplier(-value);
                break;

            case SkillType.SellPriceUp:
                PlayerStats.Instance.AddSellPriceMultiplier(value);
                break;

            case SkillType.SpecialFishChanceUp:
                PlayerStats.Instance.AddSpecialFishChance(value);
                break;

//            case SkillType.ExpBonus:
//                PlayerStats.Instance.AddExpBonus(Mathf.RoundToInt(value));
//                break;
//
//            case SkillType.ExpMultiplier:
//                PlayerStats.Instance.AddExpMultiplier(value);
//                break;
        }
    }
}