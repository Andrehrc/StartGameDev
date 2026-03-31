using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Dialogue : MonoBehaviour
{
    public float dialogueRange;
    public LayerMask playerLayer;

    public DialogueSettings dialogue;
    public GameObject actionButton;

    bool playerHit;
    private Transform playerTransform;
    private NPC npc;

    private List<string> sentences = new List<string>();
    private List<string> actorName = new List<string>();
    private List<Sprite> actorSprite = new List<Sprite>();

    void Start()
    {
        npc = GetComponent<NPC>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && playerHit)
        {
            if (sentences.Count == 0)
                GetSpeechTexts();

            actionButton.SetActive(value: false);
            npc.LookAtPlayer(playerTransform);
            DialogContol.instance.Speech(sentences.ToArray(), actorName.ToArray(), actorSprite.ToArray());
        }
    }

    void FixedUpdate()
    {
        ShowDialogue();
    }

    void ShowDialogue()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, dialogueRange, playerLayer);

        if (hit != null)
        {
            playerHit = true;
            playerTransform = hit.transform;

            if (!DialogContol.instance.IsShowing)
                actionButton.SetActive(value: true);
        }
        else if (playerHit)
        {
            playerHit = false;
            playerTransform = null;
            actionButton.SetActive(false);
            DialogContol.instance.CloseWindow();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dialogueRange);
    }

    void GetSpeechTexts()
    {
        for (int i = 0; i < dialogue.dialogues.Count; i++)
        {
            switch (DialogContol.instance.language)
            {
                case Idiom.pt:
                    sentences.Add(dialogue.dialogues[i].sentence.portugues);
                    break;

                case Idiom.eng:
                    sentences.Add(dialogue.dialogues[i].sentence.english);
                    break;

                case Idiom.esp:
                    sentences.Add(dialogue.dialogues[i].sentence.espanol);
                    break;
            }

            actorName.Add(dialogue.dialogues[i].actorName);

            actorSprite.Add(dialogue.dialogues[i].profile);
        }
    }
}
