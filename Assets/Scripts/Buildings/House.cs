using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour
{
    [Header("Amounts")]

    [SerializeField] private float timeAmount;
    [SerializeField] private int woodAmount;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Components")]
    [SerializeField] private GameObject colider;
    [SerializeField] private SpriteRenderer houseSprite;
    [SerializeField] private Transform point;

    private Player player;
    private PlayerAnim playerAnim;
    private PlayerBag bag;
    private bool detectingPlayer;
    private float timeCount;
    private bool underConstruction;
    private bool finished;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
        playerAnim = player.GetComponent<PlayerAnim>();
        bag = player.GetComponent<PlayerBag>();

    }

    void Update()
    {
        if (detectingPlayer && bag.totalWood >= woodAmount && !underConstruction && !finished)
        {
            houseSprite.color = startColor;
        }
        else if(!underConstruction && !finished)
        {
            houseSprite.color = new Color(0, 0, 0, 0);
        }

        if (detectingPlayer && Keyboard.current.eKey.wasPressedThisFrame && bag.totalWood >= woodAmount)
        {
            underConstruction = true;
            playerAnim.OnHammeringStarted();
            houseSprite.color = startColor;
            player.transform.position = point.position;
            player.isPaused = true;
            bag.totalWood -= woodAmount;
        }

        if (underConstruction)
        {
            timeCount += Time.deltaTime;

            if (timeCount > timeAmount)
            {
                playerAnim.OnHammeringEnded();
                houseSprite.color = endColor;
                player.isPaused = false;
                colider.SetActive(true);
                finished = true;
                underConstruction = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            detectingPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            detectingPlayer = false;
    }
}
