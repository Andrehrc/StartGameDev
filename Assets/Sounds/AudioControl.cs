using UnityEngine;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioClip bgmMusic;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public void StartBgmMusic()
    {
        audioManager.PlayBGM(bgmMusic);

    }
}
