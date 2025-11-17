using UnityEngine;

public class ZombieBasic : MonoBehaviour, IDamageable
{
    [Header("Status")]
    public int maxHealth = 3;
    public float moveSpeed = 1.5f;
    public int damageToPlayer = 1;

    private int currentHealth;
    private Transform target;               
    private PlayerController playerCtrl;    
    private bool isDead = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.mass = 999f;
            rb.drag = 5f;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            playerCtrl = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (isDead) return;

        
        if (playerCtrl == null || playerCtrl.IsDead)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        MoveTowardsPlayer();
        UpdateAnimations();
    }

    void MoveTowardsPlayer()
    {
        if (target == null || rb == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        Vector2 velocity = dir * moveSpeed;
        rb.velocity = velocity;

        if (spriteRenderer != null)
        {
            if (dir.x > 0.01f) spriteRenderer.flipX = false;
            else if (dir.x < -0.01f) spriteRenderer.flipX = true;
        }
    }

    void UpdateAnimations()
    {
        if (animator == null || rb == null) return;

        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (rb != null) rb.velocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Die");   

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.AddKill();

        
        Destroy(gameObject, 0.6f);
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;
        if (playerCtrl == null || playerCtrl.IsDead) return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damageToPlayer);

            if (rb != null)
                rb.velocity = Vector2.zero;
        }
    }
}
