// PlayerMovement.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Topdown.movement
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerMovement : Mover
    {
        [SerializeField] private TextMeshProUGUI levelUpText;
        public static PlayerMovement Instance;
        private Animator anim;
        private SpriteRenderer sr;
        public float PlayerMaxHealth;
        public float PlayerCurrentHealth;
        public bool IsDead = false;
        public bool IsImune;
        [SerializeField] float ImuneTimer;
        [SerializeField] float ImuneDuration;
        [SerializeField] Transform AttackPointUp; [SerializeField] float AttackUpRadius;
        [SerializeField] Transform AttackPointSide; [SerializeField] float AttackSideRadius;
        [SerializeField] Transform AttackPointDown; [SerializeField] float AttackDownRadius;
        public float Exp;
        public int CurrentLevel = 1;
        [SerializeField] private int MaxLevel;
        public int SkillPoint;
        public List<int> PlayerLevels;
        public float Damage = 2;
        private int AttackType;
        [SerializeField] LayerMask AttackLayer;

        private static readonly int FaceDown = Animator.StringToHash("FaceDown");
        private static readonly int FaceUp = Animator.StringToHash("FaceUp");
        private static readonly int SideFace = Animator.StringToHash("SideFace");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int IsRun = Animator.StringToHash("IsRun");  
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Dead = Animator.StringToHash("Dead");

        private Vector2 lastDirection = Vector2.down;

        protected override void Awake()
        {
            
                base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;

            }
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            ResetFaceDirection();
            anim.SetBool(FaceDown, true);
        }
        private void Start()
        {
            for(int i = 0; i<= MaxLevel;i++)
            {
                PlayerLevels.Add(10 * (i + 1));
            }
            UIManager.Instance.HealthBar.maxValue = PlayerMaxHealth;
            PlayerCurrentHealth = PlayerMaxHealth;
            UIManager.Instance.UpdateHealthBar();
            UIManager.Instance.UpdateLv(CurrentLevel);
        }   
        public void OnMove(InputValue value)
        {
            if (IsDead) return;
            Vector2 input = value.Get<Vector2>();

            // Lưu hướng cuối
            if (input.sqrMagnitude > 0.01f)
            {
                lastDirection = input;
            }

            UpdateFaceDirection(lastDirection);
            SetInput(input);

            // **SET isRun THEO INPUT**
            bool running = input.sqrMagnitude > 0.01f;
            anim.SetFloat(Run, input.magnitude);
            anim.SetBool(IsRun, running);  // ← QUAN TRỌNG!
        }

        public void OnAttack()
        {
            if (!anim.GetBool(IsRun) && !IsDead)
            {
                if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
                    AttackType = 1; // side
                else if (lastDirection.y > 0)
                    AttackType = 2; // up
                else
                    AttackType = 3; // down

                anim.SetTrigger(Attack);
                AudioController.Instance.PlaySound(AudioController.Instance.NormalAttack);
            }
        }
        public void OnPause()
        {
            GameManager.Instance.Pause();
        }
        
        public void OnSkillTree()
        {
            GameManager.Instance.SkillTree();
        }

        public void Die()
        {
            if (PlayerCurrentHealth <= 0)
            {
                IsDead = true;
                anim.Play("die", 0, 0.5f);
                anim.speed = 0;
                SetInput(Vector2.zero);
                enabled = false;
                GameManager.Instance.GameOver();
            }
        }

        private void UpdateFaceDirection(Vector2 dir)
        {
            ResetFaceDirection();

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                AttackType = 1;
                anim.SetBool(SideFace, true);
                sr.flipX = dir.x < 0;
            }
            else if (dir.y > 0)
            {
                AttackType = 2;
                anim.SetBool(FaceUp, true);
            }
            else
            {
                AttackType = 3;
                anim.SetBool(FaceDown, true);//AttackType(side) = 1 AttackType(up) = 2 AttackType(down) = 3
            }
        }

        private void ResetFaceDirection()
        {
            anim.SetBool(FaceDown, false);
            anim.SetBool(FaceUp, false);
            anim.SetBool(SideFace, false);
        }
        public void GetHit(float Damage)
        {
            if(!IsImune)
            {
                IsImune = true;
                ImuneTimer = ImuneDuration;
                PlayerCurrentHealth -= Damage;
                UIManager.Instance.UpdateHealthBar();
                Die();
            }
            
        }
        private void Update()
        {
            if(ImuneTimer >= 0)
            {
                ImuneTimer -= Time.deltaTime;

            }
            else
            {
                IsImune = false;
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (AttackPointDown) Gizmos.DrawWireSphere(AttackPointDown.position, AttackDownRadius);
            if (AttackPointUp) Gizmos.DrawWireSphere(AttackPointUp.position, AttackUpRadius);
            if (AttackPointSide) Gizmos.DrawWireSphere(AttackPointSide.position, AttackSideRadius);
        }
        public void OnAttackFinished()
        {
            Collider2D hit = null;

            switch (AttackType)
            {
                case 1: // Side
                    hit = GetSideAttackHit();
                    break;
                case 2: // Up
                    hit = Physics2D.OverlapCircle(AttackPointUp.position, AttackUpRadius, AttackLayer);
                    break;
                case 3: // Down
                    hit = Physics2D.OverlapCircle(AttackPointDown.position, AttackDownRadius, AttackLayer);
                    break;
            }

            if (hit != null)
            {
                var enemy = hit.GetComponent<Enemy>();
                var boss = hit.GetComponent<Boss>();

                if (enemy != null)
                    enemy.TakeDamage(Damage);
                else if (boss != null)
                    boss.TakeDamage(Damage);

                Debug.Log("Enemy hit! Damage: " + Damage);
            }

            // Reset lại để tránh tấn công liên tục nếu không đổi hướng
            AttackType = 0;
        }
        private Collider2D GetSideAttackHit()
        {
            // Tính vị trí attack bên hông dựa vào hướng đang nhìn
            float sideOffset = 0.6f;
            Vector2 attackPos = (Vector2)transform.position + new Vector2(sideOffset * (sr.flipX ? -1 : 1), 0f);

            Debug.DrawRay(attackPos, Vector2.up * 0.1f, Color.red, 0.1f);

            return Physics2D.OverlapCircle(attackPos, AttackSideRadius, AttackLayer);
        }
        public void GetExp(float ExpGet)
        {
            Exp += ExpGet;
            UIManager.Instance.UpdateExpBar();
            UIManager.Instance.UpdateLv(CurrentLevel);
            if (Exp >= PlayerLevels[CurrentLevel-1])
            {
                LevelUp();
            }
        }
        public void MaxHealthBootsUpdate(float Amount)
        {
            PlayerMaxHealth += Amount;
        }
        public void MaxAttackBootsUpdate(float Amount)
        {
            Damage += Amount;
        }
        public void MaxSpeedBoots(float Amount)
        {
            speed += Amount;
        }
        public void LevelUp()
        {
            AudioController.Instance.PlaySound(AudioController.Instance.LvUp);
            Exp -= PlayerLevels[CurrentLevel - 1];
            CurrentLevel++;
            UIManager.Instance.UpdateExpBar();
            UIManager.Instance.UpdateLv(CurrentLevel);
            DamageController.Instance.CreatelvlUp(transform.position);

        }

    }

}