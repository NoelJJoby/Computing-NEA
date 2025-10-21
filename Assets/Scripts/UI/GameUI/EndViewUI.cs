using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndViewUI : MonoBehaviour
{
    [SerializeField] private Button startNewButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button openSavePanelButton;
    [SerializeField] private Button submitSaveButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject SavingPanel;

    private void Awake()
    {
        startNewButton.onClick.AddListener(() =>
        {
            LevelDataController.SetGameOption(LevelDataController.GameOption.New);
            SceneController.MoveToScene(SceneController.SceneName.LevelSelectorScene);
        });

        restartButton.onClick.AddListener(() =>
        {
            LevelDataController.SetGameOption(LevelDataController.GameOption.Restart);
            SceneController.MoveToScene(SceneController.SceneName.GameScene);
        });

        openSavePanelButton.onClick.AddListener(() =>
        {
            if (SavingPanel.activeSelf)
            {
                SavingPanel.SetActive(false);
                openSavePanelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Open Save Panel";
            }
            else
            {
                SavingPanel.SetActive(true);
                openSavePanelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Close Save Panel";
            }

        });

        submitSaveButton.onClick.AddListener(() =>
        {
            LevelDataController.SaveLevel();
            SavingPanel.SetActive(false);
            openSavePanelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Open Save Panel";
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    private void Start()
    {
        timerText.text = "Timer: " + ((int)GameManager.Instance.GetGameTimer()).ToString();
        scoreText.text = "Score: " + ((int)GameManager.Instance.GetScore()).ToString();
    }
}
