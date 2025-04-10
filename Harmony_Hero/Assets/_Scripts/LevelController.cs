using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject reminder;

    private void Update()
    {

    }
    public void NewGameReminder()
    {
        if (reminder != null)
        {
            reminder.SetActive(true);
        }
    }
    public void NewGameStart()
    {
        SaveGameManager.Instance().SaveGame(1, 1, 0, 1);
        DataKeeper dataKeeper = GameObject.Find("DataKeeper").GetComponent<DataKeeper>();
        if (dataKeeper != null)
        {
            dataKeeper.LoadGameFromSaveTxt();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    public void WinScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void LoseScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void LevelScene()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayDefault();

        SceneManager.LoadScene(1);
    }

    public void BattleScene()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayBattle();

        DataKeeper.Instance.LoadPlayerData(GameObject.Find("Player").GetComponent<PlayerController>());
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayDefault();

        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
