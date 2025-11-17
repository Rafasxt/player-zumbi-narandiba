using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private PlayerController owner;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false; 
    }

    public void SetOwner(PlayerController player)
    {
        owner = player;
    }

    public void EnableHitbox(bool enable)
    {
        if (col != null)
            col.enabled = enable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == null) return;

        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(owner.GetCurrentAttackDamage());
        }
    }
}
