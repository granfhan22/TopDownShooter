using UnityEngine;
using System.Collections.Generic;

public class BulletSystem : MonoBehaviour
{
    public static BulletSystem Instance;

    [Header("Prefabs & Settings")]
    [SerializeField] private GameObject waterBulletPrefab;
    [SerializeField] private int poolSize = 3;
    [SerializeField] private int bulletCount = 1;                   
    [SerializeField] private float spreadAngle = 60f;               
    [SerializeField] private float searchRadius = 100f;             

    private Queue<WaterBullet> pool = new Queue<WaterBullet>();
    private List<WaterBullet> activeBullets = new List<WaterBullet>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(waterBulletPrefab, transform);
            WaterBullet bullet = obj.GetComponent<WaterBullet>();
            obj.SetActive(false);
            pool.Enqueue(bullet);
        }
    }


    public void Spawn(Vector3 position, float damage)
    {
        Transform nearestEnemy = FindNearestEnemy(position);
        if (nearestEnemy == null) return;

        Vector3 centerDir = (nearestEnemy.position - position).normalized;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (i - (bulletCount - 1f) / 2f) * (spreadAngle / (bulletCount - 1f));
            Vector3 shootDir = Quaternion.Euler(0, 0, angle) * centerDir;

            WaterBullet bullet = GetBullet();
            if (bullet != null)
            {
                bullet.SetDamage(damage);
                bullet.transform.position = position;
                bullet.transform.right = shootDir;
                bullet.Initialize(shootDir, nearestEnemy);
                activeBullets.Add(bullet);
            }
        }
    }

    private WaterBullet GetBullet()
    {
        if (pool.Count > 0)
        {
            WaterBullet bullet = pool.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        GameObject obj = Instantiate(waterBulletPrefab, transform);
        return obj.GetComponent<WaterBullet>();
    }

    public void ReturnBullet(WaterBullet bullet)
    {
        activeBullets.Remove(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        pool.Enqueue(bullet);
    }

    private Transform FindNearestEnemy(Vector3 fromPos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(fromPos, searchRadius);
        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                float dist = Vector2.Distance(fromPos, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = hit.transform;
                }
            }
        }
        return closest;
    }
}