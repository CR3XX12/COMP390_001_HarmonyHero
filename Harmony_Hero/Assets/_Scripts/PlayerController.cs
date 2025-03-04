using UnityEngine;
using UnityEngine.Windows;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private InputSystem_Actions _inputs;
    [SerializeField] private Vector2 _move;
    [SerializeField] private float _velocity;

    private LevelController _levelController;

    [SerializeField] public float _playerHealth;
    [SerializeField] public float _playerDamage;

    // XP Variables
    [SerializeField] public int _playerXP = 0;
    [SerializeField] public int _playerLevel = 1;
    [SerializeField] public int _xpToNextLevel = 100;

    // UI Elements (For Level 1 UI)
    private TextMeshProUGUI levelText;
    private Slider xpBar;
    private Slider healthBar;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new InputSystem_Actions();

        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    void Start()
    {
        _levelController = GameObject.Find("LevelController")?.GetComponent<LevelController>();
        // Load saved player data if available
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.LoadPlayerData(this);
        }

        // Find Player UI in Level 1 if it exists
        GameObject levelTextObj = GameObject.Find("Level");
        if (levelTextObj != null)
        {
            levelText = levelTextObj.GetComponent<TextMeshProUGUI>();
            levelText.text = "lv." + _playerLevel;
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

        // Delay UI Update to Prevent NULL Errors
        StartCoroutine(DelayedUIUpdate());
    }

    // Coroutine to Delay UI Update for 0.1 seconds
    private IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForSeconds(0.1f);  // Small delay to allow UIManager to initialize

        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel);
        }
    }



    void Update()
    {
        _playerDamage = _playerHealth * 0.5f;
        Vector3 movement = new Vector3(_move.x * _velocity * Time.fixedDeltaTime, 0.0f, _move.y * _velocity * Time.fixedDeltaTime);
        _controller.Move(movement);

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
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Instruction":
                break;

            case "BattlePoint":
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

    public void GainXP(int xpAmount)
    {
        _playerXP += xpAmount;

        // Update UI in Level 1 (If UI exists)
        if (xpBar != null)
        {
            xpBar.value = _playerXP;
        }
        if (levelText != null)
        {
            levelText.text = "lv." + _playerLevel;
        }

        // Update UI in BattleScene if UIManager exists
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel);
        }

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
        _playerDamage += 0.1f;  // Increase damage slightly

        // Save progress immediately after leveling up
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.SavePlayerData(this);
        }

        // Update Level 1 UI
        if (levelText != null)
        {
            levelText.text = "lv." + _playerLevel;
        }
        if (xpBar != null)
        {
            xpBar.maxValue = _xpToNextLevel;
            xpBar.value = _playerXP;
        }

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
}