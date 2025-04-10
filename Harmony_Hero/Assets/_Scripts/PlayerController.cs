// (Keep all your using statements as-is)
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private InputSystem_Actions _inputs;
    [SerializeField] private Vector2 _move;
    [SerializeField] private float _velocity = 3;
    bool isFacingRight = true;
    private LevelController _levelController;

    [SerializeField] public float _playerHealth;
    [SerializeField] public float _playerDamage;
    [SerializeField] public int _playerCurrentBattle;
    // XP Variables
    [SerializeField] public int _playerXP = 0;
    [SerializeField] public int _playerLevel = 1;
    [SerializeField] public int _xpToNextLevel = 100;
    [SerializeField] public bool isInBattle;

    // UI Elements (For Level 1 UI)
    private TextMeshProUGUI levelText;
    private Slider xpBar;
    private Slider healthBar;

    public CinemachineRotationComposer composer;
    public DataKeeper _dataKeeper;
    Animator _animator;

    // Footstep Sound
    [SerializeField] private AudioClip leftFootstep;
    [SerializeField] private AudioClip rightFootstep;
    private AudioSource _audioSource;

    // Skill Prefab
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private GameObject healPrefab;

    // Skill Sound Effect 🔊
    [SerializeField] private AudioClip skillSFX;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new InputSystem_Actions();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    void Start()
    {
        _dataKeeper = GameObject.Find("DataKeeper")?.GetComponent<DataKeeper>();
        _levelController = GameObject.Find("LevelController")?.GetComponent<LevelController>();

        if (DataKeeper.Instance != null)
        {
            StartCoroutine(LoadDataAndSetupUI());
        }
        else
        {
            SetupUI();
        }

        // Get current scene index
        isInBattle = SceneManager.GetActiveScene().buildIndex == 2;

        if (!isInBattle)
        {
            _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
            _inputs.Player.Move.canceled += context => _move = Vector2.zero;
            _inputs.Player.Attack.performed += context => LeftTurn();
            _inputs.Player.Heal.performed += context => RightTurn();
        }
    }

    private IEnumerator LoadDataAndSetupUI()
    {
        // Load player data
        DataKeeper.Instance.LoadPlayerData(this);

        // Wait for the next frame to ensure data is loaded
        yield return null;

        // Now safely set up UI
        SetupUI();
    }

    private void SetupUI()
    {
        GameObject levelTextObj = GameObject.Find("Level");
        if (levelTextObj != null)
        {
            levelText = levelTextObj.GetComponent<TextMeshProUGUI>();
            levelText.text = "lv. " + _playerLevel;
        }

        GameObject xpBarObj = GameObject.Find("XPbar");
        if (xpBarObj != null)
        {
            xpBar = xpBarObj.GetComponent<Slider>();
            xpBar.maxValue = _xpToNextLevel;
            xpBar.value = _playerXP;
        }

        GameObject healthBarObj = GameObject.Find("Health");
        if (healthBarObj != null)
        {
            healthBar = healthBarObj.GetComponent<Slider>();
            healthBar.value = _playerHealth;
        }
    }

    public void LeftTurn()
    {
        transform.Rotate(Vector3.up, -30f);
    }

    public void RightTurn()
    {
        transform.Rotate(Vector3.up, 30f);
    }

    void Update()
    {
        float lvPortionDamage = _playerLevel * 0.01f;
        float xpPortionDamage = (float)_playerXP / _xpToNextLevel;
        float hpPortionDamage = _playerHealth;

        _playerDamage = 0.1f + (lvPortionDamage * 0.3f + xpPortionDamage * 0.2f + hpPortionDamage * 0.5f) * 0.2f;
        Vector3 movement = transform.forward * _move.y * _velocity * Time.fixedDeltaTime +
                   transform.right * _move.x * _velocity * Time.fixedDeltaTime;

        _controller.Move(movement);
        float moveSpeedMagnitude = new Vector2(_move.x, _move.y).magnitude;
        _animator.SetFloat("moveSpeed", moveSpeedMagnitude);

        // Save before losing and transition
        if (_playerHealth <= 0.05f)
        {
            if (DataKeeper.Instance != null)
            {
                DataKeeper.Instance.SavePlayerData(this);
            }

            if (_levelController != null)
            {
                _levelController.LoseScene();
            }
            else
            {
                Debug.LogError("LevelController is NULL! Make sure it exists in the scene.");
            }
        }
        else if (_playerHealth >= 1.0f)
        {
            _playerHealth = 1.0f;
        }

        if (!isInBattle)
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        if ((_move.x > 0 && !isFacingRight) || (_move.x < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Instruction":
                break;

            case "BattlePoint":
                if (int.TryParse(other.name, out int parsedValue))
                {
                    _dataKeeper.enterBattle = parsedValue;
                    Debug.Log("Player entered battle at " + _dataKeeper.enterBattle);
                }

                // Save player progress before entering battle
                if (DataKeeper.Instance != null)
                {
                    DataKeeper.Instance.SavePlayerData(this);
                }

                // Find and transition to battle scene
                LevelController levelController = FindFirstObjectByType<LevelController>();
                if (levelController != null)
                {
                    levelController.BattleScene();
                }
                else
                {
                    Debug.LogError("LevelController not found! Make sure it exists in the scene.");
                }
                break;
        }
    }

    public void ActionAnimation(string option)
    {
        switch (option)
        {
            case "Idle":
                // Idle animation
                Debug.Log("Player Idle Animation");
                break;
            case "Attack":
                // Attack animation
                Debug.Log("Player Attacked Animation");
                break;
            case "Heal":
                // Heal animation
                Debug.Log("Player Healed Animation");
                CastHeal();
                break;
            case "Skill":
                // Skill animation
                Debug.Log("Player Skill Animation");
                CastSkill();
                break;
            case "Dodge":
                // Dodge animation
                Debug.Log("Player Dodged Animation");
                break;
            default:
                Debug.LogError("Invalid Animation Option!");
                break;
        }
    }

    // Instantiates the skill prefab at player's position and plays SFX
    private void CastSkill()
    {
        if (skillPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0.14f;
            Instantiate(skillPrefab, spawnPosition, transform.rotation);

            // 🔊 Play skill sound effect
            if (skillSFX != null)
            {
                _audioSource.PlayOneShot(skillSFX);
            }
        }
        else
        {
            Debug.LogWarning("Skill Prefab not assigned in inspector!");
        }
    }

    private void CastHeal()
    {
        if (healPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0.14f;
            Instantiate(healPrefab, spawnPosition, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Heal Prefab not assigned in inspector!");
        }
    }

    public void GainXP(int xpAmount)
    {
        _playerXP += xpAmount;

        UpdateUIfromManager();

        if (_playerXP >= _xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _playerXP -= _xpToNextLevel;
        _playerLevel++;
        _xpToNextLevel += _playerLevel * 50;  // Increase XP needed for next level

        _playerHealth += 0.2f;  // Increase player health slightly

        // Save progress immediately after leveling up
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.SavePlayerData(this);
        }

        UpdateUIfromManager();
    }

    public void UpdateUIfromManager()
    {
        // Update UI in BattleScene if UIManager exists
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel);
        }
        else
        {
            Debug.LogError("[PlayerController] UIManager not found after leveling up!");
        }
    }

    // 🎧 Play Footstep Sounds (called via animation events)
    public void PlayFootstep(int foot)
    {
        if (foot == 0 && leftFootstep != null)
        {
            _audioSource.PlayOneShot(leftFootstep);
        }
        else if (foot == 1 && rightFootstep != null)
        {
            _audioSource.PlayOneShot(rightFootstep);
        }
    }
}
