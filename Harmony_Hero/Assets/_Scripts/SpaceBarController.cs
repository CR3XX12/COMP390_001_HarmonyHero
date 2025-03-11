using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpaceBarController : MonoBehaviour
{
    [SerializeField] private GameObject _actionBar;
    [SerializeField] private float _actionBarValue;
    [SerializeField] public float speed;
    [SerializeField] private Button actionButton;
    [SerializeField] private GameObject battleManager;
    [SerializeField] private GameObject UIManager;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missSound;

    private AudioSource audioSource;

    private InputSystem_Actions _inputs;
    [SerializeField] GameObject _enemy;
    private void Awake()
    {
        _inputs = new InputSystem_Actions();
        _enemy = GameObject.Find("EnemyAI");
    }
    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        UIManager = GameObject.Find("UIManager");
        _actionBar = GameObject.Find("ActionBar");
        _actionBar.GetComponent<Slider>().value = 1f;

        // Setup Audio Source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Ensure 2D sound
        _inputs.Player.Jump.performed += context => PressedSpaceBar();
    }

    // Update is called once per frame
    void Update()
    {
        _actionBarValue = _actionBar.GetComponent<Slider>().value;
        if (speed > 0)
        {
            _actionBar.GetComponent<Slider>().value = Mathf.Repeat(Time.time * speed, 1f);
        }
    }

    void PressedSpaceBar()
    {
        if (battleManager.GetComponent<BattleManager>().playerMove.Count >= 6)
        {
            CheckHit();
        }
        else
        {
            SpaceBarMissed();
        }
    }

    public void BarReset()
    {
        _actionBar.GetComponent<Slider>().value = 0.0f;
        speed = 0.0f;
        StartCoroutine(ResumeSpeed());
    }

    private IEnumerator ResumeSpeed()
    {
        yield return new WaitForSeconds(0.1f);
        speed = 0.5f;
    }

    private void CheckHit()
    {
        speed = 0.0f;
        Debug.Log("Space Bar Pressed at " + Mathf.Round(_actionBarValue * 100f) / 100);
        _actionBar.GetComponent<Slider>().value = _actionBarValue;

        float actionPoint = Mathf.Round(_actionBarValue * 100f) / 100f;

        if (actionPoint >= 0.75f && actionPoint <= 0.85f)
        {
            PlayHitSound();
            Debug.Log("Perfect");
            actionButton.onClick.Invoke();
        }
        else if (actionPoint >= 0.7f && actionPoint < 0.75f)
        {
            PlayHitSound();
            Debug.Log("Pass");
            actionButton.onClick.Invoke();
        }
        else if (actionPoint > 0.85f && actionPoint < 0.9f)
        {
            PlayHitSound();
            Debug.Log("Pass");
            actionButton.onClick.Invoke();
        }
        else
        {
            SpaceBarMissed();
        }
    }

    void SpaceBarMissed()
    {
        PlayMissSound();
        Debug.Log("Miss");

        // Reset the keys when miss
        //battleManager.GetComponent<BattleManager>().ResetKeys();
        //battleManager.GetComponent<BattleManager>().ResetKeysUI();
        //BarReset();

        // Reset the battle when miss
        
        _enemy.GetComponent<EnemyController>().EnemyAttack(true);
        UIManager.GetComponent<UIManager>().battleKeys.SetActive(false);
        StartCoroutine(ResetBattleUIManager());
    }
    private IEnumerator ResetBattleUIManager()
    {
        yield return new WaitForSeconds(2f);
        battleManager.GetComponent<BattleManager>().ResetBattle();
        UIManager.GetComponent<UIManager>().ResetBattleUI();
        UIManager.GetComponent<UIManager>().StartBattle();
        
    }
    private void PlayHitSound()
    {
        if (hitSound != null)
        {
            GameObject soundObject = new GameObject("TempAudio");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();

            tempAudio.clip = hitSound;
            tempAudio.playOnAwake = false;
            tempAudio.spatialBlend = 0f; // Ensure 2D sound
            tempAudio.Play();

            Destroy(soundObject, hitSound.length);
        }
        else
        {
            Debug.LogWarning("No Hit Sound assigned!");
        }
    }

    private void PlayMissSound()
    {
        if (hitSound != null)
        {
            GameObject soundObject = new GameObject("TempAudio");
            AudioSource tempAudio = soundObject.AddComponent<AudioSource>();

            tempAudio.clip = missSound;
            tempAudio.playOnAwake = false;
            tempAudio.spatialBlend = 0f; // Ensure 2D sound
            tempAudio.Play();

            Destroy(soundObject, missSound.length);
        }
        else
        {
            Debug.LogWarning("No Miss Sound assigned!");
        }
    }

}
