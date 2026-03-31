using UnityEngine;
using UnityEngine.InputSystem;

public class Water : MonoBehaviour
{
    public GameObject actionButton;

    [SerializeField] private bool detectingPlayer;

    private PlayerBag player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerBag>();

    }

    void Update()
    {
        if (detectingPlayer && Keyboard.current.eKey.wasPressedThisFrame)
        {
            player.AddWaterPlayerBag(5f);
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
