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

    //XP varaibles
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        // Load saved player data if available
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.LoadPlayerData(this);
        }

        // Update UI after loading saved data
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel);
            uiManager.ShowXPReward(0, _playerXP, _xpToNextLevel); // Ensure XP bar starts with correct value
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

    public void GainXP(int xpAmount)
    {
        _playerXP += xpAmount;
        Debug.Log("Gained " + xpAmount + " XP!");

        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel);
            uiManager.ShowXPReward(xpAmount, _playerXP, _xpToNextLevel); // Show XP+ and update XP bar
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

        Debug.Log("Leveled up to " + _playerLevel + "! Health and Damage Increased.");

        //  Save progress immediately after leveling up
        if (DataKeeper.Instance != null)
        {
            DataKeeper.Instance.SavePlayerData(this);
        }

        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateXPUI(_playerXP, _playerLevel);
        }
    }




}
