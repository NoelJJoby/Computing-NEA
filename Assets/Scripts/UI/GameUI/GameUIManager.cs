using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameView;
    [SerializeField] private GameObject pausedView;
    [SerializeField] private GameObject endView;
    private bool paused = false;

    private void Start()
    {
        GameInputProcessor.Instance.OnGamePause += GameInputProcessor_OnGamePause;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        gameView.SetActive(false);
        pausedView.SetActive(false);
        endView.SetActive(true);
    }
    private void GameInputProcessor_OnGamePause(object sender, System.EventArgs e)
    {
        paused = !paused;
        if (paused)
        {
            gameView.SetActive(false);
            pausedView.SetActive(true);
        }
        else
        {
            gameView.SetActive(true);
            pausedView.SetActive(false);
        }
    }
}
