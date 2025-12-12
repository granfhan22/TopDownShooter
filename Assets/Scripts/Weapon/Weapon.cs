using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance;

    [Header("Prefabs")]
    public GameObject fireCirclePrefab;
    public GameObject waterBulletPrefab;
    public GameObject toxicLandPrefab;

    [Header("Cooldowns")]
    public float fireCircleCD = 6f;
    public float waterBulletCD = 4f;
    public float toxicLandCD = 8f;

    [Header("Unlocked")]
    public bool fireCircleUnlocked = false;
    public bool waterBulletUnlocked = false;
    public bool toxicLandUnlocked = false;

    [Header("Damage hiện tại (tăng khi nâng Boots)")]
    public float fireCircleDamage = 10f;
    public float waterBulletDamage = 30f;
    public float toxicLandDamage = 8f;

    [Header("Tăng damage mỗi lần nâng Boots")]
    public float fireCircleDamagePerLevel = 8f;
    public float waterBulletDamagePerLevel = 12f;
    public float toxicLandDamagePerLevel = 6f;
    private float fireTimer;
    private float waterTimer;
    private float toxicTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Fire Circle
        if (fireCircleUnlocked && fireCirclePrefab != null)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                AudioController.Instance.PlaySound(AudioController.Instance.FireCircle);
                var circle = Instantiate(fireCirclePrefab, transform.position, Quaternion.identity, transform);
                circle.GetComponent<FireCircle>().SetDamage(fireCircleDamage);
                fireTimer = fireCircleCD;
            }
        }

        if (waterBulletUnlocked && waterBulletPrefab != null)
        {
            waterTimer -= Time.deltaTime;
            if (waterTimer <= 0f)
            {
                BulletSystem.Instance.Spawn(transform.position, waterBulletDamage);
                AudioController.Instance.PlaySound(AudioController.Instance.WaterBullet);
                waterTimer = waterBulletCD;
            }
        }

        if (toxicLandUnlocked && toxicLandPrefab != null)
        {
            toxicTimer -= Time.deltaTime;
            if (toxicTimer <= 0f)
            {
                AudioController.Instance.PlaySound(AudioController.Instance.ToxicLand);
                var land = Instantiate(toxicLandPrefab, transform.position, Quaternion.identity);
                land.GetComponent<ToxicLand>().SetDamage(toxicLandDamage);
                toxicTimer = toxicLandCD;
            }
        }
    }

    // Gọi từ Skill Tree
    public void UnlockFireCircle() => fireCircleUnlocked = true;
    public void UnlockWaterBullet() => waterBulletUnlocked = true;
    public void UnlockToxicLand() => toxicLandUnlocked = true;

    public void UpgradeFireCircleBoots()
    {
        fireCircleDamage += fireCircleDamagePerLevel;
    }

    public void UpgradeWaterBulletBoots()
    {
        waterBulletDamage += waterBulletDamagePerLevel;
    }

    public void UpgradeToxicLandBoots()
    {
        toxicLandDamage += toxicLandDamagePerLevel;
    }
}