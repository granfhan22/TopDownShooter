using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkillSlot : MonoBehaviour
{
    public SkillSO Skill;
    public Image SkillIcon;
    public TextMeshProUGUI SkillLevelText;
    public int currentLevel;
    public bool IsUnlocked;
    public Button SkillButton;
    public List<SkillSlot> LinkedSlot;

    private void OnValidate()
    {
        if(SkillIcon != null && SkillLevelText != null)
        {
            UpdateUI();
        }
    }
    public void UpdateUI()
    {
        SkillIcon.sprite = Skill.SkillIcon;
        if (IsUnlocked)
        {
            SkillButton.interactable = true;
            SkillLevelText.text = currentLevel.ToString() + "/" + Skill.MaxLevel.ToString();
            SkillIcon.color = Color.white;
        }
        else
        {
            SkillButton.interactable = false;
            SkillLevelText.text = "Locked";
            SkillIcon.color = Color.gray;
        }
    }
    public void TryUpgradeSkill()
    {
        if (IsUnlocked && currentLevel < Skill.MaxLevel && SkillTreeManager.Instance.AvailablePoint >= Skill.SkillPointsRequire)
        {
            currentLevel++;
            SkillTreeManager.Instance.UpdateSkillPoint(-Skill.SkillPointsRequire);
            SkillTreeManager.Instance.ApplySkillEffect(this);
            Debug.Log(Skill.SkillPointsRequire);
            UpdateUI();
        }
        if (currentLevel == Skill.MaxLevel)
        {
            UnlockLinkedSkills();
        }
        else Debug.Log("khong du skill Point");

    }
    private void UnlockLinkedSkills()
    {
        foreach (var slot in LinkedSlot)
        {
                slot.IsUnlocked = true;
                slot.UpdateUI();
        }
    }
    }
