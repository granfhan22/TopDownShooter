
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private float damage = 30f;
    [SerializeField] private float speed = 14f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private float turnSpeed = 180f; 

    private Transform target;
    private Vector3 direction;

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), lifetime);
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 desiredDir = (target.position - transform.position).normalized;
            direction = Vector3.RotateTowards(direction, desiredDir, turnSpeed * Time.deltaTime * Mathf.Deg2Rad, 0f);
        }

        transform.right = direction;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Vector3 shootDir, Transform nearestEnemy)
    {
        direction = shootDir.normalized;
        target = nearestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
                enemy.TakeDamage(damage);
            else if (other.TryGetComponent<Boss>(out var boss))
                boss.TakeDamage(damage);

            Deactivate();
        }
    }

    private void Deactivate()
    {
        target = null;
        CancelInvoke();
        BulletSystem.Instance.ReturnBullet(this);  
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    public void SetDamage(float newDamage)
    {
        damage = newDamage;  
    }
}