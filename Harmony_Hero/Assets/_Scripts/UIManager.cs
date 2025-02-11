using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject battleKeys;
    [SerializeField] private GameObject attackTxt;
    [SerializeField] private GameObject healTxt;

    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject enemyHealth;

    [SerializeField] private GameObject battleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD").transform.Find("Health").gameObject;

        dialogue = GameObject.Find("Dialogue");
        battleKeys = GameObject.Find("BattleKeys");
        attackTxt = battleKeys.transform.Find("AttackTxt").gameObject;
        healTxt = battleKeys.transform.Find("HealTxt").gameObject;

        playerHealth.GetComponent<Slider>().value = 1f;
        enemyHealth.GetComponent<Slider>().value = 1f;

        ResetAction();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<PlayerController>()._playerHealth;
        enemyHealth.GetComponent<Slider>().value = GameObject.Find("EnemyAI").GetComponent<EnemyController>()._enemyHealth;
    }

    public void ResetAction()
    {
        battleManager.SetActive(false);
        dialogue.SetActive(true);
        battleKeys.SetActive(false);
        attackTxt.SetActive(false);
        healTxt.SetActive(false);
    }

    public void PressedAttack()
    {
        dialogue.SetActive(false);
        battleKeys.SetActive(true);
        attackTxt.SetActive(true);
        battleManager.SetActive(true);
        battleManager.GetComponent<BattleManager>().actionChosen = "Attack";
        battleManager.GetComponent<SpaceBarController>().BarReset();
    }

    public void PressedHeal()
    {
        dialogue.SetActive(false);
        battleKeys.SetActive(true);
        healTxt.SetActive(true);
        battleManager.gameObject.SetActive(true);
        battleManager.gameObject.GetComponent<BattleManager>().actionChosen = "Heal";
        battleManager.GetComponent<SpaceBarController>().speed = 0.5f;
    }

}
