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

    private Animator _animator;

    void Start()
    {
        _player = GameObject.Find("Player");
        _dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        if (_dataKeeper)
        {
            _enemyLevel = _dataKeeper.enterBattle;
            if (_enemyLevel <= 0)
                _enemyLevel = 2;
            Debug.Log("_enemyLevel == " + (_enemyLevel));
        }

        GameObject newChild = Instantiate(_prefabList[_enemyLevel - 1], this.transform.position, Quaternion.identity);
        newChild.name = "SpriteEnemy";
        newChild.transform.SetParent(this.transform);
        _animator = newChild.GetComponent<Animator>();
        _animator.Rebind();
        _animator.Update(0f);
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

        if (_dataKeeper.enterBattle == player._playerCurrentBattle && player._playerCurrentBattle <= 6)
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
            if(_player.GetComponent<PlayerController>()._playerCurrentBattle <= 6)
            {
                levelController.WinScene();
            }
            else if (_player.GetComponent<PlayerController>()._playerCurrentBattle > 6)
            {
                levelController.EndGame();
            }
        }
    }

    public void ActionAnimation(string action)
    {
        ResetAllTriggers();

        switch (action)
        {
            case "Attack":
                _animator.SetTrigger("Attack");
                break;

            case "GetAttacked":
                _animator.SetTrigger("Hit");
                break;

            case "Idle":
                _animator.SetBool("Idle", true);
                break;

            case "Death":
                _animator.SetTrigger("Dead");
                break;
        }
    }

    private void ResetAllTriggers()
    {
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Hit");
        _animator.ResetTrigger("Dead");
    }

    public void EnemyAttack(bool isSkill)
    {
        StartCoroutine(PerformAttack(isSkill));
    }

    private System.Collections.IEnumerator PerformAttack(bool isSkill)
    {
        ResetAllTriggers();
        _animator.SetBool("Idle", false);
        _animator.SetTrigger("Attack");

        // Wait for the animation to finish
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Go back to idle
        _animator.SetBool("Idle", true);
    }
}
