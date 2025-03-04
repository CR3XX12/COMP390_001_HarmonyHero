using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider xpBar;   // XP progress bar



    [SerializeField] private GameObject battleManager;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD").transform.Find("Health").gameObject;

        dialogue = GameObject.Find("Dialogue");
        enemyDialogue = GameObject.Find("EnemyDialogue");
        battleKeys = GameObject.Find("BattleKeys");
        attackTxt = battleKeys.transform.Find("AttackTxt").gameObject;
        healTxt = battleKeys.transform.Find("HealTxt").gameObject;

        if (playerHealth != null) playerHealth.GetComponent<Slider>().value = 1f;
        if (enemyHealth != null) enemyHealth.GetComponent<Slider>().value = 1f;

        // Assign UI Elements properly
        levelText = GameObject.Find("PlayerHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        xpBar = GameObject.Find("PlayerHUD")?.transform.Find("XPbar")?.GetComponent<Slider>();
                
        ResetDialogue();
        ResetAction();      
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null && GameObject.Find("Player") != null)
        {
            playerHealth.GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<PlayerController>()._playerHealth;
        }

        if (enemyHealth != null && GameObject.Find("EnemyAI") != null)
        {
            enemyHealth.GetComponent<Slider>().value = GameObject.Find("EnemyAI").GetComponent<EnemyController>()._enemyHealth;
        }
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

    public void UpdateXPUI(int xp, int level, int xpToNextLevel)
    {        

        if (levelText != null)
        {
            levelText.text = "lv." + level.ToString();           
        }
        else
        {
            Debug.LogError("[UIManager] Level Text is NULL!");
        }

        if (xpBar != null)
        {
            xpBar.maxValue = xpToNextLevel;  // Set XP bar's max value
            xpBar.value = xp;  // Set XP bar's current value            
        }
        else
        {
            Debug.LogError("[UIManager] XP Bar is NULL!");
        }
    }



}



