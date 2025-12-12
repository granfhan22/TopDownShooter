using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = ("NewSkill"), menuName = ("SkillTree/Skill"))] 
public class SkillSO : ScriptableObject
{
    public string SkillName;
    public int MaxLevel;
    public Sprite SkillIcon;
    public int SkillPointsRequire;

    [HideInInspector] public Action<SkillSlot> OnUpgrade;
}
