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

    [SerializeField] private List<Vector2> playerMove;

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
    }

    public void ImplementAction()
    {
        if (actionChosen == "Attack")
        {
            _enemy.GetComponent<EnemyController>()._enemyHealth -= _player.GetComponent<PlayerController>()._playerDamage;
        }
        else if (actionChosen == "Heal")
        {
            _player.GetComponent<PlayerController>()._playerHealth += 0.1f;
        }
    }

    private void ResetKeysUI()
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
                    playerMove.Clear();
                    pressedArrows.Clear();
                    pressedArrows = new List<bool>(new bool[6]);
                    ResetKeysUI();
                }
            }
        }

        if (playerMove.Count >= 6)
        {
            actionButton.interactable = true;
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    actionButton.onClick.Invoke();
            //}
        }
        else
        {
            actionButton.interactable = false;
        }
    }

}
