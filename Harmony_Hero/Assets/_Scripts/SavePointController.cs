using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.Windows;

public class SavePointController : MonoBehaviour
{
    public GameObject text;
    public TextMeshPro textComponent;
    private InputSystem_Actions _inputs;
    private void Awake()
    {
        _inputs = new InputSystem_Actions();
    }
    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();
    void Start()
    {
        textComponent = text.GetComponent<TextMeshPro>();
        _inputs.Player.Save.performed += context => SaveData();
    }
    private void Update()
    {
        text.transform.Rotate(Vector3.up * 30f * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        SaveData();
    }

    void SaveData()
    {
        DataKeeper dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        SaveGameManager.Instance().SaveGame(dataKeeper.savedHealth, dataKeeper.savedLevel, dataKeeper.savedXP, dataKeeper.currentBattle);

        textComponent.text = "Saved";
        Invoke("ResetText", 3f);
    }

    private void ResetText()
    {
        textComponent.text = "Save Point";
    }
}
