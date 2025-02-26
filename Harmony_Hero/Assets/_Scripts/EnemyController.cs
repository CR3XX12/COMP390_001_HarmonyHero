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
        if (_enemyHealth <= 0.05f)
        {
            // Get PlayerController and reward XP
            PlayerController player = _player.GetComponent<PlayerController>();
            player.GainXP(50);  // Reward XP for winning the battle

            // Save XP and Level progress before scene transition
            if (DataKeeper.Instance != null)
            {
                DataKeeper.Instance.SavePlayerData(player);
            }

            // Load the victory scene
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
