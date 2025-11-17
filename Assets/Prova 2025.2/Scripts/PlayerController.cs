using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 4f;
    public float superMoveSpeed = 5.5f;
    public bool canMove = true;

    [Header("Ataque")]
    public int attackDamage = 1;
    public int superAttackDamage = 2;
    public float attackCooldown = 0.4f;
    public AttackHitbox attackHitbox; 

    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Evolução")]
    public int killsToTransform = 15;
    public bool isSuperBarbarian = false;

    [Header("Referências")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private bool isDead = false;

    
    public bool IsDead => isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;

        if (attackHitbox != null)
            attackHitbox.SetOwner(this);
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        HandleAttackInput();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        HandleMovement();
    }

    
    void HandleInput()
    {
        if (!canMove || isAttacking)
        {
            horizontalInput = 0f;
            if (rb != null) rb.velocity = Vector2.zero;
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0.01f)
            spriteRenderer.flipX = false;
        else if (horizontalInput < -0.01f)
            spriteRenderer.flipX = true;
    }

    
    void HandleMovement()
    {
        float speed = isSuperBarbarian ? superMoveSpeed : moveSpeed;
        Vector2 velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = velocity;
    }

    
    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (isAttacking || isDead) return;

        if (Time.time - lastAttackTime < attackCooldown)
            return;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (attackHitbox != null)
        {
            attackHitbox.EnableHitbox(true);
            yield return new WaitForSeconds(0.1f);
            attackHitbox.EnableHitbox(false);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(attackCooldown - 0.1f);

        isAttacking = false;
    }

    public int GetCurrentAttackDamage()
    {
        return isSuperBarbarian ? superAttackDamage : attackDamage;
    }

    
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        canMove = false;
        rb.velocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        
    }

    
    public int currentKills = 0;

    public void AddKill()
    {
        currentKills++;

        if (!isSuperBarbarian && currentKills >= killsToTransform)
        {
            BecomeSuperBarbarian();
        }
    }

    void BecomeSuperBarbarian()
    {
        isSuperBarbarian = true;
    }

    
    void UpdateAnimations()
    {
        if (animator == null || rb == null) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
}
