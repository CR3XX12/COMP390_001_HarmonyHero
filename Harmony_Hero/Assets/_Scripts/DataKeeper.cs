using UnityEngine;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper Instance;

    // Player Stats
    public float savedHealth = 1f;
    public int savedXP = 0;
    public int savedLevel = 1;
    public int savedXPToNextLevel = 100;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keeps data persistent across scenes
    }

    public void SavePlayerData(PlayerController player)
    {
        savedHealth = player._playerHealth;
        savedXP = player._playerXP;
        savedLevel = player._playerLevel;
        savedXPToNextLevel = player._xpToNextLevel;
    }

    public void LoadPlayerData(PlayerController player)
    {
        player._playerHealth = savedHealth;
        player._playerXP = savedXP;
        player._playerLevel = savedLevel;
        player._xpToNextLevel = savedXPToNextLevel;
    }
}
