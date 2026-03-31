using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Idiom
{
    pt,
    eng,
    esp
}

public class DialogContol : MonoBehaviour
{
    public Idiom language;

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
    private string[] actorName;
    private Sprite[] actorProfile;

    private Player player;

    public static DialogContol instance;

    public bool IsShowing { get => _isShowing; set => _isShowing = value; }

    private void Awake()
    {
        instance = this;
        player = FindFirstObjectByType<Player>();
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
                profileSprite.sprite = actorProfile[index];
                actorNameText.text = actorName[index];
                StartCoroutine(TypeSentence());
            }
            else
            {
                CloseWindow();
            }
        }
    }

    public void Speech(string[] txt, string[] names, Sprite[] sprites)
    {
        if (_isShowing)
        {
            NextSentence();
            return;
        }

        player.isPaused = true;
        dialogueObg.SetActive(true);
        sentences = txt;
        actorName = names;
        actorProfile = sprites;
        profileSprite.sprite = actorProfile[index];
        actorNameText.text = actorName[index];
        StartCoroutine(TypeSentence());
        _isShowing = true;
    }

    public void CloseWindow()
    {
        player.isPaused = false;
        dialogueObg.SetActive(false);
        _isShowing = false;
        speechText.text = string.Empty;
        index = 0;
        sentences = null;
    }
}
