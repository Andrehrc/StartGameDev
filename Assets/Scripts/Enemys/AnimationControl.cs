using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask playerLayer;

    private PlayerAnim player;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerAnim>();
    }

    void Start()
    {
    }

    public void PlayAnim(int value)
    {
        anim.SetInteger("transition", value);
    }

    public void AttackHit()
    {
        anim.SetTrigger("hit");
    }

    public void DeathAnim()
    {
        anim.SetTrigger("death");
    }

    public void DeathAnimEnded()
    {
        Destroy(transform.parent.gameObject);
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, radius, playerLayer);

        if (hit != null)
        {
            player.OnHit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }
}
