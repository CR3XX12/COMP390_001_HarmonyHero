using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float _enemyHealth = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHealth <= 0)
        {
            LevelController levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
            levelController.WinScene();
        }
    }
}
