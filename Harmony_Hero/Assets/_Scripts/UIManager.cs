using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Windows;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        battleManager = GameObject.Find("BattleManager");
        playerHealth = GameObject.Find("PlayerHUD").transform.Find("Health").gameObject;
        enemyHealth = GameObject.Find("EnemyHUD").transform.Find("Health").gameObject;

        enemyDialogue = GameObject.Find("EnemyDialogue");

        battleKeys = GameObject.Find("BattleKeys");
        dialogue = GameObject.Find("Dialogue");
        actionTxt = GameObject.Find("ActionTxt");

        if (playerHealth != null) playerHealth.GetComponent<Slider>().value = 1f;
        if (enemyHealth != null) enemyHealth.GetComponent<Slider>().value = 1f;

        // Assign UI Elements properly
        enemyLevelText = GameObject.Find("EnemyHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        playerLevelText = GameObject.Find("PlayerHUD")?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        xpBar = GameObject.Find("PlayerHUD")?.transform.Find("XPbar")?.GetComponent<Slider>();

        ResetBattleUI();
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

        enemyLevelText.text = "lv. " + GameObject.Find("EnemyAI").GetComponent<EnemyController>()._enemyLevel.ToString();
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

    public void ResetBattleUI()
    {
        dialogue.SetActive(false);
        battleManager.SetActive(false);
        battleKeys.SetActive(false);
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
        dialogue.SetActive(true);
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
            playerLevelText.text = "lv." + level.ToString();
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



