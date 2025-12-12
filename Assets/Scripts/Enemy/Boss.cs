using Topdown.movement;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackRange1 = 2f;
    [SerializeField] private float attackRange2 = 4f;
    [SerializeField] private float attackCooldown1 = 3f;
    [SerializeField] private float attackCooldown2 = 10f;
    [SerializeField] float Dame1;
    [SerializeField] float Dame2;
    [SerializeField] float ExpGive;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private SpriteRenderer enemy;
    [SerializeField] private Transform attackPoint1;
    [SerializeField] private Transform attackPoint2;
    [SerializeField] private float attackRadius1 = 1f;
    [SerializeField] private float attackRadius2 = 2f;
    [SerializeField] LayerMask AttackLayer;

    private float currentHP;
    private float distanceToPlayer;
    private Vector2 Direction;
    private float attackTimer1;
    private float attackTimer2;
    private bool isAttacking = false;
    private int CurrentAttackType = 0;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void FixedUpdate()
    {
        if (isAttacking || currentHP <= 0)
        {
            Rb.linearVelocity = Vector2.zero;
            return;
        }

        Transform player = PlayerMovement.Instance.transform;
        distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (GameManager.Instance.IsGameActive)
        {
            if (PlayerMovement.Instance.transform.position.x < transform.position.x)
            {
                enemy.flipX = true;
            }
            else
            {
                enemy.flipX = false;
            }
            Direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
            Rb.linearVelocity = new Vector2(Direction.x*speed, Direction.y*speed);
            anim.SetBool("IsRun", true);
            // Attack checks
            TryAttack1();
            TryAttack2();
        }
        else
        {
            Rb.linearVelocity = Vector2.zero;
            anim.SetBool("IsRun", false);
        }

    }

    private void TryAttack1()
    {
        attackTimer1 += Time.deltaTime;
        if (distanceToPlayer <= attackRange1 && attackTimer1 >= attackCooldown1)
        {
            Attack(1);
            attackTimer1 = 0f;
        }
    }

    private void TryAttack2()
    {
        attackTimer2 += Time.deltaTime;
        if (distanceToPlayer <= attackRange2 && attackTimer2 >= attackCooldown2)
        {
            Attack(2);
            attackTimer2 = 0f;
        }
    }

    private void Attack(int attackType)
    {
        CurrentAttackType = attackType;
        isAttacking = true;
        Rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRun", false);
        

        if (attackType == 1)
            anim.SetTrigger("Attack1");
        else
            anim.SetTrigger("Attack2");
    }

    public void OnAttackFinished()
    {
        Collider2D hit1 = Physics2D.OverlapCircle(attackPoint1.position, attackRadius1, AttackLayer);
        Collider2D hit2 = Physics2D.OverlapCircle(attackPoint2.position, attackRadius2, AttackLayer);
        if (CurrentAttackType == 1)
        {
            if (hit1 && hit1.GetComponent<PlayerMovement>() != null)
            {
                PlayerMovement.Instance.GetHit(Dame1);
                Debug.Log("GetHit1");
            }
        }
        else if (CurrentAttackType == 2)
        {
            if (hit2 && hit2.GetComponent<PlayerMovement>() != null)
            {
                PlayerMovement.Instance.GetHit(Dame2);
                Debug.Log("GetHit2");
            }
        }
        isAttacking = false;
        anim.SetBool("IsRun", true);
        CurrentAttackType = 0;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    private void Die()
    {
        isAttacking = true;
        Rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRun", false);
        anim.SetBool("Dead", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f); // tùy animation chết bao lâu
        GameManager.Instance.Victory();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint1) Gizmos.DrawWireSphere(attackPoint1.position, attackRadius1);
        if (attackPoint2) Gizmos.DrawWireSphere(attackPoint2.position, attackRadius2);
    }
}   