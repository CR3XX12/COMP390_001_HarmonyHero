using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundEffects;

    // This method can be called from the animation event
    public void PlaySound(int index)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[AnimationSoundPlayer] No AudioSource assigned.");
            return;
        }

        if (index >= 0 && index < soundEffects.Length)
        {
            Debug.LogWarning("[AnimationSoundPlayer] Playing sound.");
            audioSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("[AnimationSoundPlayer] Invalid sound index.");
        }
    }
}
