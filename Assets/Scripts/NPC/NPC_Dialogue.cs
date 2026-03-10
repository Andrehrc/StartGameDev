using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Dialogue : MonoBehaviour
{
    public float dialogueRange;
    public LayerMask playerLayer;

    public DialogueSettings dialogue;

    bool playerHit;
    private Transform playerTransform;
    private NPC npc;

    private List<string> sentences = new List<string>();

    void Start()
    {
        GetSpeechTexts();
        npc = GetComponent<NPC>();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && playerHit)
        {
            npc.LookAtPlayer(playerTransform);
            DialogContol.instance.Speech(sentences.ToArray());
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
        }
        else
        {
            playerHit = false;
            playerTransform = null;
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
                case DialogContol.idiom.pt:
                    sentences.Add(dialogue.dialogues[i].sentence.portugues);
                    break;

                case DialogContol.idiom.eng:
                    sentences.Add(dialogue.dialogues[i].sentence.english);
                    break;

                case DialogContol.idiom.esp:
                    sentences.Add(dialogue.dialogues[i].sentence.espanol);
                    break;
            }


        }
    }
}
