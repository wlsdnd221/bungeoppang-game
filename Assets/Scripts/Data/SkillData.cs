using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    CookSpeedUp,
    SellPriceUp,
    StartMoneyUp,
    RerollCostDown,
    SpecialFishChanceUp
}

public enum SkillUnlockType
{
    None,
    TotalCollected,
    TotalSpecialCollected
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Bungeoppang/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("기본 정보")]
    public string skillId;
    public string skillName;
    [TextArea] public string description;

    [Header("UI")]
    public Sprite icon;
    public Vector2 nodePosition;

    [Header("스킬 설정")]
    public SkillType skillType;
    public int maxLevel = 5;
    public int baseCost = 100;
    public float costMultiplier = 1.5f;

    [Header("효과 수치")]
    public float valuePerLevel;

    [Header("선행 스킬")]
    public List<SkillData> prerequisiteSkills;

    [Header("해금 조건")]
    public SkillUnlockType unlockType;
    public int unlockRequiredAmount;
}