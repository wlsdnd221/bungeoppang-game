using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int exp = 0;
    [SerializeField] private int baseRequiredExp = 5;
    [SerializeField] private float requiredExpGrowth = 1.35f;

    [Header("UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image expBarFill;
    [SerializeField] private TMP_Text expText;

    [Header("Animation")]
    [SerializeField] private float fillSpeed = 8f;

    private float targetExpRatio;
    private float displayExpRatio;

    private int RequiredExp => Mathf.CeilToInt(baseRequiredExp * Mathf.Pow(requiredExpGrowth, level - 1));

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshTargetUI();
        displayExpRatio = targetExpRatio;
        ApplyUI();
    }

    private void Update()
    {
        displayExpRatio = Mathf.Lerp(
            displayExpRatio,
            targetExpRatio,
            Time.unscaledDeltaTime * fillSpeed
        );

        if (expBarFill != null)
            expBarFill.fillAmount = displayExpRatio;
    }

    public void AddExp(int baseExp = 1)
    {
        int bonus = 0;
        float multiplier = 1f;

        if (PlayerStats.Instance != null)
        {
            bonus = PlayerStats.Instance.expBonus;
            multiplier = PlayerStats.Instance.expMultiplier;
        }

        int finalExp = Mathf.CeilToInt((baseExp + bonus) * multiplier);

        exp += finalExp;

        while (exp >= RequiredExp)
        {
            exp -= RequiredExp;
            LevelUp();
        }

        RefreshTargetUI();

        Debug.Log($"EXP +{finalExp} / 현재 EXP: {exp}/{RequiredExp}");
    }

    private void LevelUp()
    {
        if (AugmentManager.Instance == null)
        {
            Debug.LogError("AugmentManager가 씬에 없습니다.");
            return;
        }

        level++;

        Debug.Log($"레벨업! 현재 레벨: {level}");

        RefreshTargetUI();
        AugmentManager.Instance.ShowAugmentPanel();
    }

    private void RefreshTargetUI()
    {
        targetExpRatio = (float)exp / RequiredExp;
        ApplyUI();
    }

    private void ApplyUI()
    {
        if (levelText != null)
            levelText.text = $"Lv. {level}";

        if (expText != null)
            expText.text = $"{exp} / {RequiredExp}";
    }

    public void ResetExp()
    {
        level = 1;
        exp = 0;

        targetExpRatio = 0f;
        displayExpRatio = 0f;

        ApplyUI();

        if (expBarFill != null)
            expBarFill.fillAmount = 0f;

        Debug.Log("ExperienceManager 초기화");
    }

}