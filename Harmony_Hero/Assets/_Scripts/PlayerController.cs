using UnityEngine;
using UnityEngine.Windows;

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
        _levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        // Load saved player data if available
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.LoadPlayerData(this);
        }

        // Update XP & Level UI after loading saved data
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
            _levelController.LoseScene();
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

        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel); // Update UI when XP is gained
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
        _xpToNextLevel += _playerLevel * 50;  // Increase XP requirement for next level

        _playerHealth += 0.2f;  // Increase player health slightly
        _playerDamage += 0.1f;  // Increase damage slightly

        // Save progress immediately after leveling up
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.SavePlayerData(this);
        }

        // Update UI after leveling up
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel);
        }
    }
}

