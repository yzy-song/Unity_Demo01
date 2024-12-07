
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isGamePaused;

    public GameConfig gameConfig;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // UIManager.Instance.ShowMainMenu();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }

    protected override void Awake()
    {
        base.Awake();
        // 其他初始化代码
    }
}
