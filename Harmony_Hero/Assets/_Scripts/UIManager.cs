using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Windows;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerActions;
    [SerializeField] private GameObject actionTxt;
    [SerializeField] private GameObject enemyDialogue;
    [SerializeField] public GameObject battleKeys;

    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject enemyHealth;

    // XP UI   
    [SerializeField] private TextMeshProUGUI enemyLevelText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Slider xpBar;   // XP progress bar



    [SerializeField] private GameObject battleManager;
    private InputSystem_Actions _inputs;

    private void Awake()
    {
        _inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Attack.performed += OnAttack;
        _inputs.Player.Heal.performed += OnHeal;
        _inputs.Player.Skill.performed += OnSkill;
    }

    private void OnDisable()
    {
        _inputs.Player.Attack.performed -= OnAttack;
        _inputs.Player.Heal.performed -= OnHeal;
        _inputs.Player.Skill.performed -= OnSkill;
        _inputs.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD")?.transform.Find("Health")?.gameObject;

        enemyDialogue = GameObject.Find("EnemyDialogue");

        battleKeys = GameObject.Find("BattleKeys");
        playerActions = GameObject.Find("PlayerActions");
        actionTxt = GameObject.Find("ActionTxt");

        if (playerHealth != null) playerHealth.GetComponent<Slider>().value = 1f;
        if (enemyHealth != null) enemyHealth.GetComponent<Slider>().value = 1f;

        // Assign UI Elements properly
        enemyLevelText = GameObject.Find("EnemyHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        playerLevelText = GameObject.Find("PlayerHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        xpBar = GameObject.Find("PlayerHUD")?.transform.Find("XPbar")?.GetComponent<Slider>();
        OnDisable();
        ResetBattleUI();
        StartBattle();
        enemyLevelText.text = "lv. " + GameObject.Find("EnemyAI").GetComponent<EnemyController>()._enemyLevel.ToString();
       

    }


    private void OnAttack(InputAction.CallbackContext context) => PressedOption("Attack");
    private void OnHeal(InputAction.CallbackContext context) => PressedOption("Heal");
    private void OnSkill(InputAction.CallbackContext context) => PressedOption("Skill");

    // Update is called once per frame
    void Update()
    {
        playerLevelText.text = "lv. " + GameObject.Find("DataKeeper").GetComponent<DataKeeper>().savedLevel.ToString();
        if (playerHealth != null && GameObject.Find("Player") != null)
        {
            playerHealth.GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<PlayerController>()._playerHealth;
        }

        if (enemyHealth != null && GameObject.Find("EnemyAI") != null)
        {
            enemyHealth.GetComponent<Slider>().value = GameObject.Find("EnemyAI").GetComponent<EnemyController>()._enemyHealth;
        }
    }

    public void ResetBattleUI()
    {
        playerActions.SetActive(false);
        battleManager.SetActive(false);
        battleKeys.SetActive(false);
        OnDisable();
    }

    public void StartBattle()
    {
        actionTxt.SetActive(false);
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
        yield return new WaitForSeconds(2f);

        ResetDialogueContent("Steady now! Let the music rise...");
        yield return new WaitForSeconds(2f);

        ResetDialogueContent("For the song begins and time won¡¦t wait...");
        yield return new WaitForSeconds(2f);

        EnterBattle();
    }

    private void ResetDialogueContent(string message)
    {
        enemyDialogue.transform.Find("Content").gameObject.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void ShowBattleOptions()
    {
        ResetBattleUI();
        playerActions.SetActive(true);
        OnEnable();
    }

    public void PressedOption(string choice)
    {
        Debug.Log("Pressed " + choice);
        actionTxt.SetActive(true);
        switch (choice)
        {
            case "Attack":               
                actionTxt.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = "Strike the Chords of Reckoning";
                break;
            case "Heal":
                actionTxt.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = "Weave a Melody of Renewal";
                break;
            case "Skill":
                actionTxt.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = "Unleash the Harmonic Crescendo";
                break;
            default:
                Debug.LogError("[UIManager] Invalid choice!");
                break;
        }

        battleManager.GetComponent<BattleManager>().actionChosen = choice;
        battleManager.GetComponent<BattleManager>().ImplementAction();
        ResetBattleUI();
        Invoke("StartBattle", 4f);
    }

    public void UpdateXPUI(int xp, int level, int xpToNextLevel)
    {

        if (playerLevelText != null)
        {
            playerLevelText.text = "lv. " + level.ToString();
        }
        else
        {
            Debug.LogError("[UIManager] Level Text is NULL!");
        }

        if (xpBar != null)
        {
            xpBar.maxValue = xpToNextLevel;
            xpBar.value = xp;          
        }
        else
        {
            Debug.LogError("[UIManager] XP Bar is NULL!");
        }
    }



}



