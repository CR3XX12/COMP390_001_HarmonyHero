using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject battleKeys;
    [SerializeField] private GameObject attackTxt;
    [SerializeField] private GameObject healTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = GameObject.Find("Dialogue");
        battleKeys = GameObject.Find("BattleKeys");
        attackTxt = battleKeys.transform.Find("AttackTxt").gameObject;
        healTxt = battleKeys.transform.Find("HealTxt").gameObject;

        ResetAction();
    }

    public void ResetAction()
    {
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
        battleKeys.transform.Find("BattleManager").gameObject.SetActive(true);
    }

    public void PressedHeal()
    {
        dialogue.SetActive(false);
        battleKeys.SetActive(true);
        healTxt.SetActive(true);
        battleKeys.transform.Find("BattleManager").gameObject.SetActive(true);
    }

}
