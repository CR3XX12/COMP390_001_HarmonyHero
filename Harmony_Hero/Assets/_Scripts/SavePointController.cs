using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavePointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DataKeeper dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        SaveGameManager.Instance().SaveGame(dataKeeper.savedHealth, dataKeeper.savedLevel, dataKeeper.savedXP);
    }
}
