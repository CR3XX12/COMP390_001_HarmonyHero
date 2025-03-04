using UnityEngine;

public class BattlePathController : MonoBehaviour
{

    [SerializeField] public GameObject battlePath1;
    [SerializeField] public GameObject battlePath2;
    [SerializeField] public GameObject battlePath3;
    [SerializeField] public GameObject battlePath4;
    [SerializeField] public GameObject battlePath5;
    [SerializeField] public GameObject battlePath6;

    [SerializeField] public BoxCollider battlePoint1;
    [SerializeField] public BoxCollider battlePoint2;
    [SerializeField] public BoxCollider battlePoint3;
    [SerializeField] public BoxCollider battlePoint4;
    [SerializeField] public BoxCollider battlePoint5;
    [SerializeField] public BoxCollider battlePoint6;

    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battlePath1.SetActive(true);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateBattlePaths();
    }
    void ActivateBattlePaths()
    {
        // Ensure playerController is not null
        if (playerController == null)
        {
            Debug.LogError("playerController is null!");
            return;
        }

        int battleIndex = playerController._playerCurrentBattle;

        // Store battle paths in an array for cleaner code
        GameObject[] battlePaths = { battlePath1, battlePath2, battlePath3, battlePath4, battlePath5, battlePath6 };
        BoxCollider[] battlePoints = { battlePoint1, battlePoint2, battlePoint3, battlePoint4, battlePoint5, battlePoint6 };

        // Enable paths up to the battle index
        for (int i = 0; i < battlePaths.Length; i++)
        {
            bool setActive;
            if (i <= battleIndex)
            {
                setActive = true;
            }
            else
            {
                setActive = false;
            }

            battlePaths[i].SetActive(i <= battleIndex);
            battlePoints[i].enabled = setActive;
        }
    }

}
