using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float attackCooldown = 0.6f;
    public AttackHitbox attackHitbox; 

    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Evolução / Super Bárbaro")]
    public int killsToTransform = 10;     
    public bool isSuperBarbarian = false;

    [Header("Estado do Jogo")]
    public bool hasWon = false;           

    [Header("Referências")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 inputDir;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private bool isDead = false;
    private int facingDir = 1; 

    public bool IsDead => isDead; 

    
    public int currentKills = 0;

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
        if (isDead || hasWon) return;

        HandleInput();
        HandleAttackInput();
        UpdateAnimations();
        UpdateAttackHitboxFlip();
    }

    void FixedUpdate()
    {
        if (isDead || hasWon) return;
        HandleMovement();
    }

    
    void HandleInput()
    {
        if (!canMove || isAttacking)
        {
            inputDir = Vector2.zero;
            rb.velocity = Vector2.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");   

        inputDir = new Vector2(h, v).normalized;

        
        if (h > 0.01f)
        {
            facingDir = 1;
            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
        }
        else if (h < -0.01f)
        {
            facingDir = -1;
            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
        }
    }

    
    void HandleMovement()
    {
        float speed = isSuperBarbarian ? superMoveSpeed : moveSpeed;
        rb.velocity = inputDir * speed;
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
        if (isAttacking || isDead || hasWon) return;

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

        float hitWindow = 0.2f; 
        if (attackHitbox != null)
        {
            attackHitbox.EnableHitbox(true);
            yield return new WaitForSeconds(hitWindow);
            attackHitbox.EnableHitbox(false);
        }
        else
        {
            yield return new WaitForSeconds(hitWindow);
        }

        float rest = Mathf.Max(0f, attackCooldown - hitWindow);
        if (rest > 0f)
            yield return new WaitForSeconds(rest);

        isAttacking = false;
    }

    public int GetCurrentAttackDamage()
    {
        return isSuperBarbarian ? superAttackDamage : attackDamage;
    }

    
    public void TakeDamage(int amount)
    {
        if (isDead || hasWon) return;

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
            animator.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("GAME OVER: o jogador morreu.");

        
        SceneManager.LoadScene("GameOverScene");
    }

    
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
        if (isSuperBarbarian) return;

        isSuperBarbarian = true;

        
        if (animator != null)
            animator.SetBool("IsSuper", true);

        
        transform.localScale *= 1.2f;

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1f, 0.85f, 0.6f); 
    }

    
    public void SetWin()
    {
        if (hasWon) return;
        hasWon = true;
        canMove = false;
        rb.velocity = Vector2.zero;
    }

    
    void UpdateAnimations()
    {
        if (animator == null || rb == null) return;

        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    
    void UpdateAttackHitboxFlip()
    {
        if (attackHitbox == null) return;

        float offsetX = 0.5f;
        float offsetY = -0.1f;

        attackHitbox.transform.localPosition =
            new Vector3(offsetX * facingDir, offsetY, 0);
    }
}
