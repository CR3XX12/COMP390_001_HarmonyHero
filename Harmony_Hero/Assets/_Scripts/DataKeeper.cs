using UnityEngine;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper Instance;

    // Player Stats
    public float savedHealth = 1f;
    public int savedXP = 0;
    public int savedLevel;
    public int savedXPToNextLevel;
    public int currentBattle;

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

    private void Start()
    {
        LoadGameFromSaveTxt();
    }

    public void LoadGameFromSaveTxt()
    {
        PlayerData playerData = SaveGameManager.Instance().LoadGame();

        if (playerData != null)
        {
            Debug.Log("From Load Game");
            Debug.Log("Health: " + playerData.health);
            Debug.Log("Level: " + playerData.level);
            Debug.Log("XP: " + playerData.Xp);

            savedHealth = float.Parse(playerData.health);
            savedLevel = int.Parse(playerData.level);
            savedXP = int.Parse(playerData.Xp);

            savedXPToNextLevel = savedLevel * 50;

            currentBattle = int.Parse(playerData.battle);
        }
        else
        {
            Debug.Log("No saved game data found.");
        }
    }

    public void SavePlayerData(PlayerController player)
    {
        savedHealth = player._playerHealth;
        savedXP = player._playerXP;
        savedLevel = player._playerLevel;
        savedXPToNextLevel = player._xpToNextLevel;
        currentBattle = player._playerCurrentBattle;
    }

    public void LoadPlayerData(PlayerController player)
    {
        
        if (savedHealth > 0)
        {
            player._playerHealth = savedHealth;
        }
        else
        {
            player._playerHealth = 0.1f;
        }
        player._playerXP = savedXP;
        player._playerLevel = savedLevel;
        player._xpToNextLevel = savedXPToNextLevel;
        player._playerCurrentBattle = currentBattle;
    }
}
