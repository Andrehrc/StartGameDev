using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    public GameObject actionButton;

    private bool detectingPlayer;
    [SerializeField] private int percentage;
    [SerializeField] private GameObject fishPrefab;

    private PlayerBag player;
    private PlayerAnim playerAnim;
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerBag>();
        playerAnim = player.GetComponent<PlayerAnim>();
    }

    void Update()
    {
        if (detectingPlayer && Keyboard.current.eKey.wasPressedThisFrame)
        {
            playerAnim.OnCastingStarted();
        }
    }

    public void OnCasting()
    {
        int randomValue = Random.Range(1, 100);
        if (randomValue <= percentage)
        {
            float randomY = Random.Range(0.4f, 0.8f);

            var fishSpawnPosition = player.transform.position + new Vector3(-1f, randomY, 0f);
            Instantiate(fishPrefab, fishSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Não Pescou");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectingPlayer = true;
            actionButton.SetActive(value: true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectingPlayer = false;
            actionButton.SetActive(value: false);
        }
    }
}
