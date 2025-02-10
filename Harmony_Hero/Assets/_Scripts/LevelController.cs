using UnityEngine;

public class LevelController : MonoBehaviour
{
    public void NewGameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LevelScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void BattleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
