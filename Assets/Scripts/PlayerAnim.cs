using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLayer;

    private Player player;
    private Animator anim;
    private SpriteRenderer sr;
    private Casting casting;

    private bool _rollTriggered;
    private bool _fishingTriggered;
    private bool isHitting;

    private float recoveryTime = 1.5f;
    private float timeCount;

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        casting = FindFirstObjectByType<Casting>();
    }

    void Start()
    {

    }

    void Update()
    {
        OnMove();
    }

    #region Movement
    private void OnMove()
    {
        if (player.isRolling)
        {
            if (!_rollTriggered)
            {
                anim.SetTrigger("isRoll");
                _rollTriggered = true;
            }

            return;
        }

        _rollTriggered = false;

        if (player.IsFishing)
        {
            return;
        }

        _fishingTriggered = false;

        if (player.direction.sqrMagnitude > 0)
            anim.SetInteger("transition", player.isRunning ? 2 : 1);
        else
            anim.SetInteger("transition", value: 0);

        if (player.direction.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (player.direction.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        if (player.isCutting)
            anim.SetInteger("transition", value: 3);

        if (player.isDigging)
            anim.SetInteger("transition", value: 4);

        if (player.isWatering)
            anim.SetInteger("transition", value: 5);

        if (player.IsAttacking)
            anim.SetInteger("transition", value: 6);

        if (isHitting)
        {
            timeCount += Time.deltaTime;

            if (timeCount >= recoveryTime)
            {
                isHitting = false;
                timeCount = 0f;
            }
        }
    }

    public void OnCastingStarted()
    {
        if (!_fishingTriggered)
        {
            player.IsFishing = true;

            if (player.direction.sqrMagnitude > 0)
                player.lockDirection = player.direction.normalized;

            anim.SetTrigger("isCasting");
            _fishingTriggered = true;
        }
    }

    public void OnCastingEnded()
    {
        casting.OnCasting();
    }

    public void OnCastingAnimationEnded()
    {
        player.IsFishing = false;
        player.ResetSpeed();
    }

    public void OnHammeringStarted()
    {
        anim.SetBool("hammering", true);
    }

    public void OnHammeringEnded()
    {
        anim.SetBool("hammering", false);

    }

    public void OnHit()
    {
        if (!isHitting)
        {
            anim.SetTrigger("hit");
            isHitting = true;
        }
    }
    #endregion

    #region Attack

    public void OnAttack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, radius, enemyLayer);

        if (hit != null)
        {
            Skeleton enemy = hit.GetComponent<Skeleton>();
            enemy.PlayerAttackHit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

    #endregion
}
