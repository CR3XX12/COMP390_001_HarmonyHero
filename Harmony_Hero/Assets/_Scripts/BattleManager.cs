using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _enemy;
    private InputSystem_Actions _inputs;
    [SerializeField] private Vector2 _move;

    [SerializeField] private Color unpressed;
    [SerializeField] private Color pressed;
    [SerializeField] private GameObject[] arrows;

    [SerializeField] public List<Vector2> playerMove;

    private Vector2[] moveOptions;
    [SerializeField] private Vector2[] moveSelected;
    private List<bool> pressedArrows = new List<bool>(new bool[6]);

    [SerializeField] private Button actionButton;
    [SerializeField] public string actionChosen;


    private void Awake()
    {
        _player = GameObject.Find("Player");
        _enemy = GameObject.Find("EnemyAI");
        _inputs = new InputSystem_Actions();

    }
    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.performed += context => playerMove.Add(_move);
        // animation according to key pressed
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;

        unpressed = new Color(96 / 255f, 66 / 255f, 0.0f, 1.0f);
        pressed = new Color(217 / 255f, 174 / 255f, 81 / 255f, 1.0f);

        ResetKeysUI();

        moveOptions = new Vector2[4];
        moveOptions[0] = new Vector2(0, 1); //up
        moveOptions[1] = new Vector2(0, -1); //down
        moveOptions[2] = new Vector2(1, 0); //right
        moveOptions[3] = new Vector2(-1, 0); //left

        InitiateMoves();
        AssignArrow();

        actionChosen = "Idle";

    }



    public void ImplementAction()
    {
        // player action animation
        _player.GetComponent<PlayerController>().ActionAnimation(actionChosen);

        if (actionChosen == "Idle")
        {
            _enemy.GetComponent<EnemyController>().EnemyAttack(false);
        }
        else if (actionChosen == "Attack")
        {
            Debug.Log("Player Attacked");
            _enemy.GetComponent<EnemyController>().ActionAnimation("GetAttacked");
            _enemy.GetComponent<EnemyController>()._enemyHealth -= _player.GetComponent<PlayerController>()._playerDamage;
            _enemy.GetComponent<EnemyController>().EnemyAttack(false);
            _player.GetComponent<PlayerController>().ActionAnimation("Dodge");
        }
        else if (actionChosen == "Heal")
        {
            Debug.Log("Player Healed");
            _player.GetComponent<PlayerController>()._playerHealth += 0.1f;
            _enemy.GetComponent<EnemyController>().ActionAnimation("Wait");
        }
        else if (actionChosen == "Skill")
        {
            Debug.Log("Player Skill");
            _enemy.GetComponent<EnemyController>().ActionAnimation("GetAttacked");
            _enemy.GetComponent<EnemyController>()._enemyHealth -= _player.GetComponent<PlayerController>()._playerDamage * 2;
            _enemy.GetComponent<EnemyController>().EnemyAttack(false);
            _player.GetComponent<PlayerController>().ActionAnimation("Dodge");
        }
    }

    public void ResetKeysUI()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.GetComponent<Image>().color = unpressed;
        }
    }

    private void InitiateMoves()
    {
        moveSelected = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            moveSelected[i] = moveOptions[Random.Range(0, 4)];
        }
    }

    private void AssignArrow()
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Atan2(moveSelected[i].y, moveSelected[i].x) * Mathf.Rad2Deg;
            arrows[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    public void ResetBattle()
    {
        playerMove.Clear();
        pressedArrows.Clear();
        pressedArrows = new List<bool>(new bool[6]);
        ResetKeysUI();
        InitiateMoves();
        AssignArrow();
        gameObject.SetActive(false);
    }
    public void ResetKeys()
    {
        playerMove.Clear();
        pressedArrows.Clear();
        pressedArrows = new List<bool>(new bool[6]);
    }
    void Update()
    {
        if (_move != Vector2.zero)
        {
            for (int i = 0; i < Mathf.Min(moveSelected.Length, playerMove.Count, pressedArrows.Count); i++)
            {
                if (playerMove[i] == moveSelected[i] && !pressedArrows[i])
                {
                    arrows[i].GetComponent<Image>().color = pressed;
                    pressedArrows[i] = true;
                }
                else if (playerMove[i] != moveSelected[i] && !pressedArrows[i])
                {
                    ResetKeys();
                    ResetKeysUI();
                }
            }
        }

        if (playerMove.Count >= 6)
        {
            actionButton.interactable = true;

        }
        else
        {
            actionButton.interactable = false;
        }


    }

}
