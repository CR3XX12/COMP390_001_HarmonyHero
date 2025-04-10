using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip battleMusic;

    private AudioSource _audio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _audio = GetComponent<AudioSource>();
            _audio.loop = true;
            _audio.clip = defaultMusic;
            _audio.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDefault()
    {
        if (_audio.clip != defaultMusic)
        {
            _audio.volume = 1f;
            _audio.clip = defaultMusic;
            _audio.Play();
        }
    }

    public void PlayBattle()
    {
        if (_audio.clip != battleMusic)
        {
            _audio.volume = 0.4f;
            _audio.clip = battleMusic;
            _audio.Play();
        }
    }
}
