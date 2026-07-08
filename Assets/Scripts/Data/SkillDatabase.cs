using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Bungeoppang/Skill Database")]
public class SkillDatabase : ScriptableObject
{
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    public List<SkillData> GetAllSkills()
    {
        return skills;
    }

    public SkillData GetSkillById(string skillId)
    {
        foreach (SkillData skill in skills)
        {
            if (skill.skillId == skillId)
            {
                return skill;
            }
        }

        Debug.LogWarning($"Skill not found: {skillId}");
        return null;
    }
}