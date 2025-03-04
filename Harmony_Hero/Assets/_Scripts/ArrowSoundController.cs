using UnityEngine;

public class ArrowSoundController : MonoBehaviour
{
    public AudioClip arrowClip; // Assign an MP3/WAV/OGG in Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Audio settings
        audioSource.clip = arrowClip;
        audioSource.volume = 1f; // Max volume
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Ensure 2D playback
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        if (arrowClip != null)
        {
            audioSource.PlayOneShot(arrowClip);
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }
    }
}
