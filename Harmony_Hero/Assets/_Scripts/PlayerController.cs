﻿using UnityEngine;
using UnityEngine.Windows;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


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
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new InputSystem_Actions();
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    void Start()
    {
        _dataKeeper = GameObject.Find("DataKeeper")?.GetComponent<DataKeeper>();
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

        //get current scene index
        isInBattle = SceneManager.GetActiveScene().buildIndex == 2;

        if (!isInBattle)
        {
            _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
            _inputs.Player.Move.canceled += context => _move = Vector2.zero;
            _inputs.Player.Attack.performed += context => LeftTurn();
            _inputs.Player.Heal.performed += context => RightTurn();
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
    // Coroutine to Delay UI Update for 0.1 seconds
    private IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForSeconds(0.1f);  // Small delay to allow UIManager to initialize

        //UIManager uiManager = FindFirstObjectByType<UIManager>();

        //if (uiManager != null)
        //{
        //    uiManager.UpdateXPUI(_playerXP, _playerLevel, _xpToNextLevel);
        //}
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
        if(!isInBattle)
        { FlipSprite(); }
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
                break;
            case "Skill":
                // Skill animation
                Debug.Log("Player Skill Animation");
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