using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject enemyDialogue;
    [SerializeField] private GameObject battleKeys;
    [SerializeField] private GameObject attackTxt;
    [SerializeField] private GameObject healTxt;

    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject enemyHealth;

    // XP UI
    [SerializeField] private Text xpText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text xpGainText; // XP+ text (e.g., "+50")
    [SerializeField] private Slider xpBar;   // XP progress bar


    [SerializeField] private GameObject battleManager;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD").transform.Find("Health").gameObject;

        dialogue = GameObject.Find("Dialogue");
        enemyDialogue = GameObject.Find("EnemyDialogue");
        battleKeys = GameObject.Find("BattleKeys");
        attackTxt = battleKeys.transform.Find("AttackTxt").gameObject;
        healTxt = battleKeys.transform.Find("HealTxt").gameObject;

        playerHealth.GetComponent<Slider>().value = 1f;
        enemyHealth.GetComponent<Slider>().value = 1f;

        // Correctly assign Level and XP UI elements
        levelText = GameObject.Find("PlayerHUD").transform.Find("Level").GetComponent<Text>();
        xpText = GameObject.Find("PlayerHUD").transform.Find("XP+").GetComponent<Text>();
        xpBar = GameObject.Find("PlayerHUD").transform.Find("XPbar").GetComponent<Slider>();

        xpGainText.gameObject.SetActive(false); // Hide XP+ by default

        ResetDialogue();
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
        enemyDialogue.SetActive(true);
        battleManager.SetActive(false);
        battleKeys.SetActive(false);
        attackTxt.SetActive(false);
        healTxt.SetActive(false);
        Invoke("ResetDialogue", 2f);
    }

    public void ResetDialogue()
    {
        enemyDialogue.SetActive(false);
        dialogue.SetActive(true);
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

    // Update XP UI (XP text and level)
    public void UpdateXPUI(int xp, int level)
    {
        xpText.text = "XP: " + xp;
        levelText.text = "Level: " + level;
    }

    // Show XP+ text briefly after battle and update XP bar
    public void ShowXPReward(int xpGained, int currentXP, int xpToNextLevel)
    {
        xpGainText.text = "+" + xpGained;
        xpGainText.gameObject.SetActive(true); // Show XP+
        xpBar.value = (float)currentXP / xpToNextLevel; // Update XP bar

        StartCoroutine(HideXPText()); // Hide XP+ after a few seconds
    }

    private IEnumerator HideXPText()
    {
        yield return new WaitForSeconds(2f); // Wait 2 seconds
        xpGainText.gameObject.SetActive(false); // Hide XP+
    }
}



