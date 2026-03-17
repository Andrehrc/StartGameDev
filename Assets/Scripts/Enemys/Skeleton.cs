using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    [Header("Stats")]
    public float totalLife;
    public float skeletonLife;
    public Image healthBar;


    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AnimationControl animControl;

    private Player player;

    private bool isDead;
    private bool isTakingHit;

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

        agent.SetDestination(player.transform.position);

        if (Vector2.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            animControl.PlayAnim(2);
        else
            animControl.PlayAnim(1);

        float posX = player.transform.position.x - transform.position.x;

        if (posX > 0)
            transform.eulerAngles = new Vector2(0, 0);
        else
            transform.eulerAngles = new Vector2(0, 180);
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
}
