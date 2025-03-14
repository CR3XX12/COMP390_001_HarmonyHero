using TMPro;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public GameObject text;
    public GameObject instruction;
    public GameObject reminder;

    private DataKeeper dataKeeper;
    void Start()
    {
        dataKeeper = Object.FindFirstObjectByType<DataKeeper>();
        instruction.SetActive(false);

        switch (dataKeeper.currentBattle)
        {
            case 2:
                reminder.GetComponent<TextMeshPro>().text = "The 2nd Verse Begins!\nTune your melody!";
                break;
            case 3:
                reminder.GetComponent<TextMeshPro>().text = "The 3rd Chorus Awaits!\nStrike the right chord!";
                break;
            case 4:
                reminder.GetComponent<TextMeshPro>().text = "The 4th Harmony Rises!\nSing your strength!";
                break;
            case 5:
                reminder.GetComponent<TextMeshPro>().text = "The 5th Refrain Echoes!\nYour music guides you.";
                break;
            case 6:
                reminder.GetComponent<TextMeshPro>().text = "The Final Crescendo!\nLet your song shine!";
                break;
            default:
                reminder.GetComponent<TextMeshPro>().text = "Can you hear\nthe melody of fate?";
                break;
        }


    }
    private void Update()
    {
        text.transform.Rotate(Vector3.down * 100f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Instruction");
        instruction.SetActive(true);
        Invoke("CloseInstruction", 2f);
    }

    void CloseInstruction()
    {
        instruction.SetActive(false);
    }
}
