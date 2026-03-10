using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogContol : MonoBehaviour
{
    [System.Serializable]
    public enum idiom
    {
        pt,
        eng,
        esp
    }

    public idiom language;

    [Header("Components")]
    public GameObject dialogueObg;
    public Image profileSprite;
    public Text speechText;
    public Text actorNameText;

    [Header("Settings")]
    public float typingSpeed;

    bool _isShowing;
    int index;
    private string[] sentences;

    public static DialogContol instance;

    public bool IsShowing { get => _isShowing; set => _isShowing = value; }

    private void Awake()
    {
        instance = this;
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if (speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechText.text = string.Empty;
                StartCoroutine(TypeSentence());
            }
            else
            {
                CloseWindow();
            }
        }
    }

    public void Speech(string[] txt)
    {
        if (_isShowing)
        {
            NextSentence();
            return;
        }

        dialogueObg.SetActive(true);
        sentences = txt;
        StartCoroutine(TypeSentence());
        _isShowing = true;
    }

    public void CloseWindow()
    {
        dialogueObg.SetActive(false);
        _isShowing = false;
        speechText.text = string.Empty;
        index = 0;
        sentences = null;
    }
}
