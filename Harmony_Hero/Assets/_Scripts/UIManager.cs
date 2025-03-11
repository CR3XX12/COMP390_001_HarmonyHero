using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Windows;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject enemyDialogue;
    [SerializeField] private GameObject battleKeys;
    [SerializeField] private GameObject attackTxt;
    [SerializeField] private GameObject healTxt;
    [SerializeField] private GameObject skillTxt;

    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject enemyHealth;

    // XP UI   
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider xpBar;   // XP progress bar



    [SerializeField] private GameObject battleManager;
    private InputSystem_Actions _inputs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD").transform.Find("Health").gameObject;

        enemyDialogue = GameObject.Find("EnemyDialogue");

        battleKeys = GameObject.Find("BattleKeys");
        dialogue = GameObject.Find("Dialogue");
        attackTxt = dialogue.transform.Find("AttackTxt").gameObject;
        healTxt = dialogue.transform.Find("HealTxt").gameObject;
        skillTxt = dialogue.transform.Find("SkillTxt").gameObject;

        if (playerHealth != null) playerHealth.GetComponent<Slider>().value = 1f;
        if (enemyHealth != null) enemyHealth.GetComponent<Slider>().value = 1f;

        // Assign UI Elements properly
        levelText = GameObject.Find("PlayerHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        xpBar = GameObject.Find("PlayerHUD")?.transform.Find("XPbar")?.GetComponent<Slider>();

        ResetBattle();
        StartBattle();

        _inputs = new InputSystem_Actions();
    }
    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();
    private void Start()
    {
        _inputs.Player.Attack.performed += context => PressedOption("Attack");
        _inputs.Player.Heal.performed += context => PressedOption("Heal");
        _inputs.Player.Skill.performed += context => PressedOption("Skill");
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

    public void ResetBattle()
    {
        dialogue.SetActive(false);
        battleManager.SetActive(false);
        battleKeys.SetActive(false);
        attackTxt.SetActive(false);
        healTxt.SetActive(false);
        skillTxt.SetActive(false);
    }

    public void StartBattle()
    {
        enemyDialogue.SetActive(true);
        StartCoroutine(ShowDialogueSequence());
    }

    public void EnterBattle()
    {
        enemyDialogue.SetActive(false);
        battleKeys.SetActive(true);
        battleManager.SetActive(true);
        battleManager.GetComponent<SpaceBarController>().speed = 0.5f;
    }
    private IEnumerator ShowDialogueSequence()
    {
        ResetDialogueContent("Your melody falters before the abyss...");
        yield return new WaitForSeconds(2f); // Wait 1 second

        ResetDialogueContent("Steady now! Let the music rise...");
        yield return new WaitForSeconds(2f); // Wait 1 second

        ResetDialogueContent("For the song begins and time won¡¦t wait...");
        yield return new WaitForSeconds(2f); // Wait 1 second

        EnterBattle();
    }

    private void ResetDialogueContent(string message)
    {
        enemyDialogue.transform.Find("Content").gameObject.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void ShowBattleOptions()
    {
        ResetBattle();
        dialogue.SetActive(true);
    }

    public void PressedOption(string choice)
    {
        GameObject choiceUI = null;

        switch (choice)
        {
            case "Attack":
                choiceUI = attackTxt;
                break;
            case "Heal":
                choiceUI = healTxt;
                break;
            case "Skill":
                choiceUI = skillTxt;
                break;
            default:
                Debug.LogError("[UIManager] Invalid choice!");
                break;
        }

        choiceUI.SetActive(true);
        battleManager.GetComponent<BattleManager>().actionChosen = choice;
        battleManager.GetComponent<BattleManager>().ImplementAction();
        ResetBattle();
        Invoke("StartBattle", 2f);

       
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



