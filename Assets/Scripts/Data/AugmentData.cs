using UnityEngine;

public enum AugmentEffectType
{
    SellPriceMultiplier,     // 판매 가격 배율
    CookTimeMultiplier,      // 굽기 시간 배율
    ExpMultiplier,           // 경험치 배율
    ExpBonus                 // 추가 경험치
}

public enum AugmentRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(
    fileName = "AugmentData",
    menuName = "Bungeoppang/Augment Data"
)]
public class AugmentData : ScriptableObject
{
    [Header("기본 정보")]
    public string augmentId;
    public string augmentName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("분류")]
    public AugmentRarity rarity;
    public AugmentEffectType effectType;

    [Header("효과 수치")]
    public float value;
}