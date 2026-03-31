using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public AudioControl audioControl;
    public Player player;
    public GameObject canvas;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
        Time.timeScale = 0f;
        player.isPaused = true;
    }

    public void StartGame()
    {
        audioControl.StartBgmMusic();
        player.isPaused = false;
        Time.timeScale = 1f;
        canvas.SetActive(true);
    }

    public void ChangeLanguage(int languague)
    {
        DialogContol.instance.language = (Idiom)languague;
    }

}
