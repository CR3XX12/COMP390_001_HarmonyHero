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

    [SerializeField] public Light battleLight1;
    [SerializeField] public Light battleLight2;
    [SerializeField] public Light battleLight3;
    [SerializeField] public Light battleLight4;
    [SerializeField] public Light battleLight5;
    [SerializeField] public Light battleLight6;

    private DataKeeper dataKeeper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battlePath1.SetActive(true);
        dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        ActivateBattlePaths();
    }

    // Update is called once per frame
    void Update()
    {
        //ActivateBattlePaths();
    }
    void ActivateBattlePaths()
    {
        // Ensure playerController is not null
        if (dataKeeper == null)
        {
            Debug.LogError("playerController is null!");
            return;
        }

        int battleIndex = dataKeeper.currentBattle;

        // Store battle paths in an array for cleaner code
        GameObject[] battlePaths = { battlePath1, battlePath2, battlePath3, battlePath4, battlePath5, battlePath6 };
        BoxCollider[] battlePoints = { battlePoint1, battlePoint2, battlePoint3, battlePoint4, battlePoint5, battlePoint6 };
        Light[] battleLights = { battleLight1, battleLight2, battleLight3, battleLight4, battleLight5, battleLight6 };
        // Enable paths up to the battle index
        for (int i = 0; i < battlePaths.Length; i++)
        {
            bool setActive;
            if (i < battleIndex)
            {
                setActive = true;
            }
            else
            {
                setActive = false;
            }

            battlePaths[i].SetActive(i < battleIndex);
            battlePoints[i].enabled = setActive;
        }
        if(battleIndex - 1 < battleLights.Length)
        { battleLights[battleIndex - 1].GetComponent<Light>().color = Color.white; }
    }

}
