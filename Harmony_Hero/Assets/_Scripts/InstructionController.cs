using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class InstructionController : MonoBehaviour
{
    public GameObject text;
    public GameObject instruction;
    void Start()
    {
        instruction.SetActive(false);
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
