using TMPro;
using UnityEngine;

public class SkillTooltipUI : MonoBehaviour
{
    public static SkillTooltipUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text effectText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(SkillData skill)
    {
        panel.SetActive(true);

        int level = SkillManager.Instance.GetSkillLevel(skill);

        nameText.text = skill.skillName;
        descriptionText.text = skill.description;
        effectText.text = $"효과: {skill.valuePerLevel}";
        levelText.text = $"레벨 {level}/{skill.maxLevel}";

        if (SkillManager.Instance.IsMaxLevel(skill))
        {
            costText.text = "최대치";
        }
        else
        {
            int cost = SkillManager.Instance.GetUpgradeCost(skill);
            costText.text = $"비용: {cost}원";
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}