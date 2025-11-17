using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 4f;
    public float superMoveSpeed = 6f;
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
    public float superDuration = 20f;   
    float superTimer;
    Vector3 originalScale;
    Color originalColor;

    [Header("UI")]
    public TMP_Text killsText;     
    public TMP_Text timerText;     
    public float matchDuration = 300f; 
    float timeRemaining;
    bool timerEnded = false;

    [Header("Referências")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    Vector2 inputDir;
    bool isAttacking = false;
    float lastAttackTime = -999f;
    bool isDead = false;
    bool hasWon = false;

    int facingDir = 1;

    public bool IsDead => isDead;

    public int currentKills = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;

        currentHealth = maxHealth;

        if (attackHitbox != null)
            attackHitbox.SetOwner(this);

        // timer
        timeRemaining = matchDuration;

        // reset stats globais
        currentKills = 0;
        GameStats.Kills = 0;
        GameStats.PlayerWon = false;

        UpdateKillsUI();
        UpdateTimerUI();
    }

    void Update()
    {
        HandleTimer();

        if (isSuperBarbarian)
        {
            superTimer -= Time.deltaTime;
            if (superTimer <= 0f)
            {
                EndSuperBarbarian();
            }
        }

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

    
    void HandleTimer()
    {
        if (timerEnded) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0) timeRemaining = 0;

        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            timerEnded = true;

            GameStats.Kills = currentKills;
            GameStats.PlayerWon = false;

            SceneManager.LoadScene("GameOverScene");
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        int min = seconds / 60;
        int sec = seconds % 60;

        timerText.text = $"{min:00}:{sec:00}";
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
            spriteRenderer.flipX = false;
        }
        else if (h < -0.01f)
        {
            facingDir = -1;
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

        float rest = attackCooldown - hitWindow;
        if (rest > 0)
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

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        canMove = false;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("Die");

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        GameStats.Kills = currentKills;
        GameStats.PlayerWon = false;

        SceneManager.LoadScene("GameOverScene");
    }

    
    public void AddKill()
    {
        currentKills++;
        GameStats.Kills = currentKills;

        UpdateKillsUI();

        if (!isSuperBarbarian && currentKills >= killsToTransform)
        {
            BecomeSuperBarbarian();
        }
    }

    void UpdateKillsUI()
    {
        if (killsText != null)
            killsText.text = currentKills.ToString();
    }

    void BecomeSuperBarbarian()
    {
        if (isSuperBarbarian) return;

        isSuperBarbarian = true;
        superTimer = superDuration;

        animator.SetBool("IsSuper", true);

        transform.localScale = originalScale * 1.2f;

        spriteRenderer.color = new Color(1f, 0.85f, 0.6f);
    }

    void EndSuperBarbarian()
    {
        isSuperBarbarian = false;

        animator.SetBool("IsSuper", false);

        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;
    }

    public void SetWin()
    {
        if (hasWon) return;

        hasWon = true;
        canMove = false;
        rb.velocity = Vector2.zero;

        GameStats.Kills = currentKills;
        GameStats.PlayerWon = true;
    }

    
    void UpdateAnimations()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
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
