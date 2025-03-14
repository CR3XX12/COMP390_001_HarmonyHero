using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public GameObject[] _prefabList;
    [SerializeField] public float _enemyHealth = 1f;
    [SerializeField] public int _enemyLevel = 1;
    [SerializeField] public float _enemyDamage = 0.1f;
    [SerializeField] public GameObject _player;
    [SerializeField] public DataKeeper _dataKeeper;

    void Start()
    {
        _player = GameObject.Find("Player");
        _dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        if (_dataKeeper)
        { _enemyLevel = _dataKeeper.enterBattle; }

        GameObject newChild = Instantiate(_prefabList[_enemyLevel - 1], this.transform.position, Quaternion.identity);
        newChild.name = "SpriteEnemy";
        newChild.transform.SetParent(this.transform);
        Animator animator = this.GetComponent<Animator>();
        animator.Rebind();
        animator.Update(0f);
    }

    void Update()
    {
        if (_enemyHealth <= 0f)
        {
            StartCoroutine(ChangeScene());
        }

    }

    private IEnumerator ChangeScene()
    {
        ActionAnimation("Dead");
        yield return new WaitForSeconds(2f);

        // Get PlayerController and reward XP
        PlayerController player = _player.GetComponent<PlayerController>();
        player.GainXP(50);  // Reward XP for winning the battle

        if (player._playerCurrentBattle < 6)
        {
            player._playerCurrentBattle++;
        }

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

    public void ActionAnimation(string option)
    {
        // enemy action animation
        if (option == "Attack")
        {
            // enemy attack animation
            Debug.Log("Enemy Attacked Animation");
        }
        else if (option == "GetAttacked")
        {
            // enemy get attacked animation
            Debug.Log("Enemy GetAttacked Animation");
        }
        else if (option == "Wait")
        {
            // enemy wait animation
            Debug.Log("Enemy Waited Animation");
        }
        else if (option == "Dead")
        {
            // enemy dead animation
            Debug.Log("Enemy Dead Animation");
        }
    }

    public void EnemyAttack(bool validAttack)
    {
        this.ActionAnimation("Attack");

        if (validAttack)
        {
            float maxEmemyDamage = _enemyLevel * 0.1f;
            _enemyDamage = Mathf.Round(Random.Range(0.1f, maxEmemyDamage) * 10) / 10f; //standard rounding to 1 decimal place
            _player.GetComponent<PlayerController>()._playerHealth -= _enemyDamage;
        }

    }
}
