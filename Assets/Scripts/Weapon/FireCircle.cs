using UnityEngine;

public class FireCircle : MonoBehaviour
{
    [SerializeField] private float Damage = 10f;
    [SerializeField] private float duration = 5f;         
    [SerializeField] private float growSpeed = 3f;         

    private float timer;
    private float damageTimer;
    private Vector3 targetScale = Vector3.one * 2f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        timer = duration;
        damageTimer = 0f;
    }

    private void Update()
    {
        // Phóng to
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.deltaTime * growSpeed);

        // Đếm ngược thời gian tồn tại
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            targetScale = Vector3.zero;
            if (Vector3.Distance(transform.localScale, Vector3.zero) < 0.05f)
            {
                gameObject.SetActive(false); // hoặc Destroy nếu không dùng pool
                // Destroy(gameObject);
            }
        }

        // Đếm thời gian gây damage định kỳ
        damageTimer -= Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (damageTimer > 0) return;
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.TakeDamage(Damage);

            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                Boss boss = collision.GetComponent<Boss>();
                boss.TakeDamage(Damage);
            }
            damageTimer = 1;
        }
    }
    public void SetDamage(float dmg)
    {
        Damage = dmg;
    }
}
