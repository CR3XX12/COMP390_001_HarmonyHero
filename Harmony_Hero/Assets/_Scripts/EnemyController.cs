using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float _enemyHealth = 1f;
    [SerializeField] public float _enemyDamage = 0.1f;
    [SerializeField] public bool isAttacked;
    [SerializeField] public GameObject _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAttacked = false;
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHealth <= 0)
        {
            LevelController levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
            levelController.WinScene();
        }

        if (isAttacked)
        {
            EnemyAttack();
        }

    }

    public void EnemyAttack()
    {
        // enemy attack animation
        _enemyDamage = Random.Range(0.1f, 0.5f);
        _player.GetComponent<PlayerController>()._playerHealth -= _enemyDamage;
        isAttacked = false;
    }
}
