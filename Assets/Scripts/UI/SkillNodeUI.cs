using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillNodeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SkillData skill;
    private bool isPointerOver = false;

    [Header("Node UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockObject;

    [Header("Tooltip")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TMP_Text tooltipNameText;
    [SerializeField] private TMP_Text tooltipDescriptionText;
    [SerializeField] private TMP_Text tooltipLevelText;
    [SerializeField] private TMP_Text tooltipCostText;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        if (skill != null)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (skill.icon != null)
        {
            iconImage.sprite = skill.icon;
        }

        bool isUnlocked = SkillManager.Instance.IsUnlocked(skill);
        bool hasPrerequisites = SkillManager.Instance.HasPrerequisites(skill);
        bool isLocked = !isUnlocked || !hasPrerequisites;

        lockObject.SetActive(isLocked);

        if (isLocked)
        {
            button.interactable = false;
            return;
        }

        if (SkillManager.Instance.IsMaxLevel(skill))
        {
            button.interactable = false;
            return;
        }

        int cost = SkillManager.Instance.GetUpgradeCost(skill);
        button.interactable = MoneyManager.Instance.CanSpendMoney(cost);
    }

    // 클릭이벤트
    private void OnClick()
    {
        SkillManager.Instance.UpgradeSkill(skill);
        SkillTreeUI.Instance.Refresh();
        if (isPointerOver)
        {
            UpdateTooltip();
        }
    }
    // 포인터 ON
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        UpdateTooltip();
        tooltipPanel.SetActive(true);

    }
    // 포인터 OUT
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        tooltipPanel.SetActive(false);
    }

    public void UpdateTooltip()
    {
        int level = SkillManager.Instance.GetSkillLevel(skill);

        tooltipNameText.text = skill.skillName;
        tooltipDescriptionText.text = skill.description;
        tooltipLevelText.text = $"{level}/{skill.maxLevel}";

        if (SkillManager.Instance.IsMaxLevel(skill))
        {
            tooltipCostText.text = "MAX";
        }
        else
        {
            int cost = SkillManager.Instance.GetUpgradeCost(skill);
            tooltipCostText.text = $"{cost}";
        }
    }

    // 초기화 함수
    public void Init(SkillData skillData)
    {
        skill = skillData;
//        Refresh();
    }
}