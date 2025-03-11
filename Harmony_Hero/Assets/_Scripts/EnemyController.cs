using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float _enemyHealth = 1f;
    [SerializeField] public float _enemyDamage = 0.1f;
    [SerializeField] public GameObject _player;

    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        if (_enemyHealth <= 0f)
        {
            // Get PlayerController and reward XP
            PlayerController player = _player.GetComponent<PlayerController>();
            player.GainXP(50);  // Reward XP for winning the battle
            player._playerCurrentBattle++;

            // Ensure XP and Level progress are saved before transitioning
            if (DataKeeper.Instance != null)
            {
                DataKeeper.Instance.SavePlayerData(player);
            }

            // player win and enemy lose animation
            // change scene after all animation done

            // Find and call LevelController to load the Win Scene
            LevelController levelController = FindFirstObjectByType<LevelController>();
            if (levelController != null)
            {
                levelController.WinScene(); // Load WinScene
            }
        }

    }

    public void ActionAnimation(string option)
    {
        // enemy action animation
        if (option == "Attack")
        {
            // enemy attack animation
            Debug.Log("Enemy Attacked");
        }
        else if (option == "Wait")
        {
            // enemy wait animation
            Debug.Log("Enemy Waited");
        }
    }

    public void EnemyAttack(bool validAttack)
    {
        this.ActionAnimation("Attack");

        if (validAttack)
        {
            _enemyDamage = Random.Range(0.1f, 0.5f);
            _player.GetComponent<PlayerController>()._playerHealth -= _enemyDamage;
        }
        else
        {
            _player.GetComponent<PlayerController>().ActionAnimation("Dodge");
        }
    }
}
