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
    }

    // Update is called once per frame
    void Update()
    {
        _playerDamage = _playerHealth * 0.5f;
        Vector3 movement = new Vector3(_move.x * _velocity * Time.fixedDeltaTime, 0.0f, _move.y * _velocity * Time.fixedDeltaTime);
        _controller.Move(movement);

        if (_playerHealth <= 0.05f)
        {
            _levelController.LoseScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Instruction":
                Debug.Log("Instruction");
                break;
            case "BattlePoint":
                _levelController.BattleScene();
                Debug.Log("BattlePoint");
                break;
        }
    }

}
