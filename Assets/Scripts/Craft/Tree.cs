using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private float treeHealth;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private ParticleSystem leafs;

    private Transform player;
    private bool isCut;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnHit()
    {
        treeHealth--;

        anim.SetTrigger("isHit");
        leafs.Play();

        float direction = player.position.x < transform.position.x ? 1f : -1f;

        if (treeHealth <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                float randomY = Random.Range(-0.5f, 0.5f);
                float randomX = Random.Range(0.3f, 0.7f) * direction;

                var woodSpawnPosition = transform.position + new Vector3(randomX, randomY, 0f);
                var wood = Instantiate(woodPrefab, woodSpawnPosition, transform.rotation);

                wood.GetComponent<Wood>().SetDirection(direction);
            }

            anim.SetTrigger("cut");
            isCut = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCut)
            return;

        if (collision.CompareTag("Axe"))
            OnHit();
    }
}
