using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    [Header("Stats")]
    public float radius;
    [HideInInspector] public float totalLife;
    public float skeletonLife;
    public Image healthBar;


    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AnimationControl animControl;
    [SerializeField] private Transform standPosition;

    public LayerMask layer;

    private Player player;

    private bool isDead;
    private bool isTakingHit;
    private bool detectPlayer;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        totalLife = skeletonLife;
    }

    void Update()
    {
        if (isDead || isTakingHit) return;

        if (!detectPlayer)
        {
            agent.stoppingDistance = 0f;
            float distanceToStand = Vector2.Distance(transform.position, standPosition.position);

            if (distanceToStand > 0.1f)
            {
                agent.isStopped = false;
                agent.SetDestination(standPosition.position);
                animControl.PlayAnim(1);
            }
            else
            {
                agent.isStopped = true;
                animControl.PlayAnim(0);
            }

            HandleFlip();
            return;
        }

        agent.stoppingDistance = 1.5f;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);

        if (Vector2.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            animControl.PlayAnim(2);
        else
            animControl.PlayAnim(value: 1);

        float posX = player.transform.position.x - transform.position.x;

        if (posX > 0)
            transform.eulerAngles = new Vector2(0, 0);
        else
            transform.eulerAngles = new Vector2(0, 180);
    }

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    private void HandleFlip()
    {
        float moveX = agent.velocity.x;

        if (moveX > 0.01f)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (moveX < -0.01f)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void PlayerAttackHit()
    {
        if (isDead || isTakingHit) return;

        skeletonLife--;

        healthBar.fillAmount = (float)skeletonLife / totalLife;

        if (skeletonLife <= 0)
        {
            isDead = true;
            agent.isStopped = true;
            animControl.DeathAnim();
            return;
        }

        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        isTakingHit = true;
        agent.isStopped = true;

        animControl.AttackHit();

        yield return new WaitForSeconds(0.4f);

        agent.isStopped = false;
        isTakingHit = false;
    }

    public void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, layer);

        if (hit != null)
        {
            detectPlayer = true;
        }
        else
        {
            detectPlayer = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
