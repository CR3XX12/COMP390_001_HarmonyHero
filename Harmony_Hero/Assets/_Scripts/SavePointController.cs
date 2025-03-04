using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class SavePointController : MonoBehaviour
{
    public GameObject text;
    public TextMeshPro textComponent;

    void Start()
    {
        textComponent = text.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        text.transform.Rotate(Vector3.up * 30f * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        DataKeeper dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        SaveGameManager.Instance().SaveGame(dataKeeper.savedHealth, dataKeeper.savedLevel, dataKeeper.savedXP);

        textComponent.text = "Saved";
        Invoke("ResetText", 3f);
    }

    private void ResetText()
    {
        textComponent.text = "Save Point";
    }
}
