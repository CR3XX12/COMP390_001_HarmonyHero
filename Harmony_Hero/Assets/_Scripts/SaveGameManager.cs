using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string health;
    public string level;
    public string Xp;
}

[System.Serializable]
public class SaveGameManager
{
    private static SaveGameManager m_instance = null;
    private SaveGameManager() { }
    public static SaveGameManager Instance()
    {
        return m_instance ??= new SaveGameManager();
    }
    private string _filePath = Application.persistentDataPath + "/MySaveData.txt";
    public void SaveGame(float health, int level, int Xp )
    {
        var binaryFormatter = new BinaryFormatter();
        var file = File.Create(_filePath);

        var data = new PlayerData
        {
            level = level.ToString(),
            Xp = Xp.ToString(),
            health = health.ToString(),
        };
        binaryFormatter.Serialize(file, data);
        file.Close();
        Debug.Log("Game Data Save");
        Debug.Log(_filePath);
    }

    public PlayerData LoadGame()
    {
        if (!File.Exists(_filePath)) { return null; }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(_filePath, FileMode.Open);
        PlayerData data = formatter.Deserialize(file) as PlayerData;
        file.Close();
        return data;
    }

}
