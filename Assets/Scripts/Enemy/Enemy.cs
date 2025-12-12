using Topdown.movement;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemy;
    [SerializeField] Rigidbody2D Rb;
    private Vector3 Direction;
    [SerializeField] float Dame;
    [SerializeField] float health;
    [SerializeField] float ExpGive;

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameActive)
        {
            if (PlayerMovement.Instance.transform.position.x > transform.position.x)
            {
                enemy.flipX = true;
            }
            else
            {
                enemy.flipX = false;
            }
            Direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
            Rb.linearVelocity = new Vector2(Direction.x, Direction.y);
        }
        else
        {
            Rb.linearVelocity = Vector2.zero;
        }
    }
    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerMovement.Instance.GetHit(Dame);
        }
    }
    public void TakeDamage(float damage)
    {
        DamageController.Instance.CreateNumber(damage, transform.position);
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
            PlayerMovement.Instance.GetExp(ExpGive);
            AudioController.Instance.PlayModifiedSound(AudioController.Instance.EnemyDead);
        }
    }
}
