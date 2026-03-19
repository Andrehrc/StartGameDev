using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotFarm : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip holeSFX;
    [SerializeField] private AudioClip carrotSFX;


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
    private bool plantedCarrot;
    private bool playerDetected;

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

            if (currentWater >= waterAmount && !plantedCarrot)
            {
                spriteRenderer.sprite = carrot;
                plantedCarrot = true;
                audioSource.PlayOneShot(holeSFX);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame && plantedCarrot && playerDetected)
            {
                audioSource.PlayOneShot(carrotSFX);

                spriteRenderer.sprite = hole;
                bag.carrots++;
                currentWater = 0;
                plantedCarrot = false;
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
            audioSource.PlayOneShot(holeSFX);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shovel"))
            OnHit();

        if (collision.CompareTag("Water"))
            detecting = true;

        if (collision.CompareTag("Player"))
            playerDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
            detecting = false;

        if (collision.CompareTag("Player"))
            playerDetected = false;
    }
}
