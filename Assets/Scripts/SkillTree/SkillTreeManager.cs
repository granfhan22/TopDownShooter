using TMPro;
using Topdown.movement;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public SkillSlot[] SkillSlots;
    public TextMeshProUGUI PointText;
    public int AvailablePoint ;
    public FireCircle fire;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject); 
    }
    private void Start()
    {
        foreach(SkillSlot slot in SkillSlots)
        {
            slot.SkillButton.onClick.AddListener(slot.TryUpgradeSkill);
            OnSkillUpgrade(slot);
        }
        UpdateSkillPoint(0);
    }
    public void UpdateSkillPoint(int Amount)
    {
        AvailablePoint += Amount;
        PointText.text = "SkillPoint: " + AvailablePoint;
    }
    private void OnSkillUpgrade(SkillSlot slot)
    {
        string SkillName = slot.Skill.SkillName;
        switch(SkillName)
        {
            case "MaxHealthBoots":
                slot.Skill.OnUpgrade = (s) => PlayerMovement.Instance.MaxHealthBootsUpdate(10);
                break;
            case "MaxAttackBoots":
                slot.Skill.OnUpgrade = (s) => PlayerMovement.Instance.MaxAttackBootsUpdate(10);
                break;
            case "SpeedBoots":
                slot.Skill.OnUpgrade = (s) => PlayerMovement.Instance.MaxSpeedBoots(0.5f);
                break;
            case "FireCircle":
                slot.Skill.OnUpgrade = (s) =>
                {
                    if (s.currentLevel == 1)
                        Weapon.Instance.UnlockFireCircle();
                };
                break;

            case "WaterBullet":
                slot.Skill.OnUpgrade = (s) =>
                {
                    if (s.currentLevel == 1)
                        Weapon.Instance.UnlockWaterBullet();
                };
                break;

            case "ToxicLand":
                slot.Skill.OnUpgrade = (s) =>
                {
                    if (s.currentLevel == 1)
                        Weapon.Instance.UnlockToxicLand();
                };
                break;
            case "FireCircleBoots":
                slot.Skill.OnUpgrade = (s) => Weapon.Instance.UpgradeFireCircleBoots();
                break;
            case "WaterBulletBoots":
                slot.Skill.OnUpgrade = (s) => Weapon.Instance.UpgradeWaterBulletBoots();
                break;
            case "ToxicLandBoots":
                slot.Skill.OnUpgrade = (s) => Weapon.Instance.UpgradeToxicLandBoots();
                break;

        }
    }
    public void ApplySkillEffect(SkillSlot slot)
    {
        slot.Skill.OnUpgrade?.Invoke(slot);
    }
}
