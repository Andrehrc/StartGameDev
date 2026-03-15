using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotFarm : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite hole;
    [SerializeField] private Sprite carrot;

    [Header("Settings")]
    [SerializeField] private int digAmount;
    [SerializeField] private float waterAmount;

    [SerializeField] private bool detecting;

    private int initialDigAmount;
    private float currentWater;

    private bool dugHole;

    PlayerBag bag;

    void Start()
    {
        initialDigAmount = digAmount;
        bag = FindFirstObjectByType<PlayerBag>();
    }

    void Update()
    {
        if (dugHole)
        {
            if (detecting)
                currentWater += 0.01f;

            if (currentWater >= waterAmount)
            {
                spriteRenderer.sprite = carrot;

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    spriteRenderer.sprite = hole;
                    bag.carrots++;
                    currentWater = 0;
                }
            }
        }
    }

    public void OnHit()
    {
        digAmount--;

        if (digAmount == 0)
        {
            spriteRenderer.sprite = hole;
            dugHole = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shovel"))
            OnHit();

        if (collision.CompareTag("Water"))
            detecting = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
            detecting = false;
    }
}
