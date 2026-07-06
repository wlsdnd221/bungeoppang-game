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

    [Header("Exp Bonus")]
    [SerializeField] private int expBonus = 0;
    [SerializeField] private float expMultiplier = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image expBarFill;

    [Header("Animation")]
    [SerializeField] private float fillSpeed = 8f;

    private float targetExpRatio;
    private float displayExpRatio;

    private int RequiredExp =>
        Mathf.CeilToInt(baseRequiredExp * Mathf.Pow(requiredExpGrowth, level - 1));

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
        int finalExp = Mathf.CeilToInt((baseExp + expBonus) * expMultiplier);

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
        if (AugmentUI.Instance == null)
        {
            Debug.LogError("AugmentUI가 씬에 없습니다.");
            return;
        }

        level++;

        Debug.Log($"레벨업! 현재 레벨: {level}");

        // 2순위에서 여기서 레벨업 UI/증강창 띄움
        RefreshTargetUI();
        AugmentUI.Instance.Show();
    }

    public void AddExpBonus(int amount)
    {
        expBonus += amount;
    }

    public void AddExpMultiplier(float amount)
    {
        expMultiplier += amount;
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
    }
}