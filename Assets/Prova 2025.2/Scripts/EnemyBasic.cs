using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBasic : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public int maxHealth = 3;
    public float moveSpeed = 1.5f;
    public int damageToPlayer = 1;

    [Tooltip("Quão perto ele para do player (visual)")]
    public float stopDistance = 0.3f;

    [Tooltip("Raio em que o golpe acerta o player")]
    public float attackRange = 0.5f;

    [Tooltip("Tempo entre um golpe e outro")]
    public float attackCooldown = 1.0f;

    [Header("Referências (opcional)")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private int currentHealth;
    private Rigidbody2D rb;
    private Transform player;
    private PlayerController playerCtrl;
    private bool isDead = false;
    private float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCtrl = playerObj.GetComponent<PlayerController>();
        }

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.OnEnemySpawned();
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null || playerCtrl == null) return;

        if (playerCtrl.IsDead)
        {
            rb.velocity = Vector2.zero;
            SetSpeedParam(0f);
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > stopDistance)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            SetSpeedParam(rb.velocity.magnitude);
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetSpeedParam(0f);
            TryAttack(dist);
        }

        FlipSprite();
    }

    void TryAttack(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange) return;

        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (!playerCtrl.IsDead)
            playerCtrl.TakeDamage(damageToPlayer);
    }

    void FlipSprite()
    {
        if (spriteRenderer == null || player == null) return;

        float dx = player.position.x - transform.position.x;
        if (dx > 0.01f) spriteRenderer.flipX = false;
        else if (dx < -0.01f) spriteRenderer.flipX = true;
    }

    void SetSpeedParam(float value)
    {
        if (animator == null) return;
        animator.SetFloat("Speed", value);
    }

    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        SetSpeedParam(0f);

        if (animator != null)
            animator.SetTrigger("Die");

        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.AddKill();

        
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.OnEnemyKilled();
        }

        Destroy(gameObject, 0.6f);
    }
}
