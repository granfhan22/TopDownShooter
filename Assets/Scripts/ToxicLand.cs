using UnityEngine;

public class ToxicLand : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage = 8f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private float duration = 5f;           
    [SerializeField] private float growSpeed = 4f;

    private float timer;
    private float damageTimer;
    private Vector3 targetScale = Vector3.one * 2.5f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        timer = duration;
        damageTimer = 0f;
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * growSpeed);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            targetScale = Vector3.zero;
            if (Vector3.Distance(transform.localScale, Vector3.zero) < 0.05f)
            {
                gameObject.SetActive(false);
            }
        }

        damageTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
                enemy.TakeDamage(damage);
            else if (other.TryGetComponent<Boss>(out var boss))
                boss.TakeDamage(damage);

            damageTimer = damageInterval;
        }
    }
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
}