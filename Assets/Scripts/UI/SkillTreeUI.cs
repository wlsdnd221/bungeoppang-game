using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    public static SkillTreeUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text moneyText;

    [Header("자동 생성")]
    [SerializeField] private SkillDatabase skillDatabase;
    [SerializeField] private SkillNodeUI skillNodePrefab;
    [SerializeField] private Transform nodeParent;

    private readonly List<SkillNodeUI> skillNodes = new List<SkillNodeUI>();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        CreateSkillNodes();
        Close();
    }

    private void CreateSkillNodes()
    {
        Debug.Log($"스킬 DB 개수: {skillDatabase.GetAllSkills().Count}");

        foreach (SkillData skill in skillDatabase.GetAllSkills())
        {
            Debug.Log($"스킬 노드 생성: {skill.skillName}, 위치: {skill.nodePosition}");

            SkillNodeUI node = Instantiate(skillNodePrefab, nodeParent);
            node.Init(skill);

            RectTransform rect = node.GetComponent<RectTransform>();
            rect.anchoredPosition = skill.nodePosition;

            skillNodes.Add(node);
            node.Refresh();
        }
    }

    public void Open()
    {
        panel.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Refresh()
    {
        moneyText.text = $"{MoneyManager.Instance.GetMoney()}원";

        foreach (SkillNodeUI node in skillNodes)
        {
            node.Refresh();
        }
    }
}