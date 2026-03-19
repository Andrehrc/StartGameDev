using UnityEngine;

public class ActorSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSouce;

    public void PlaySFX(AudioClip clip)
    {
        audioSouce.PlayOneShot(clip);
    }
}
