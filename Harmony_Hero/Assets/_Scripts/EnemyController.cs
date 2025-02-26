using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float _enemyHealth = 1f;
    [SerializeField] public float _enemyDamage = 0.1f;
    [SerializeField] public bool isAttacked;
    [SerializeField] public GameObject _player;

    void Start()
    {
        isAttacked = false;
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        if (_enemyHealth <= 0f) 
        {
            // Get PlayerController and reward XP
            PlayerController player = _player.GetComponent<PlayerController>();
            player.GainXP(50);  // Reward XP for winning the battle

            // Ensure XP and Level progress are saved before transitioning
            if (DataKeeper.Instance != null)
            {
                DataKeeper.Instance.SavePlayerData(player);
            }

            // Find and call LevelController to load the Win Scene
            LevelController levelController = FindFirstObjectByType<LevelController>();
            if (levelController != null)
            {
                levelController.WinScene(); // Load WinScene
            }
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
